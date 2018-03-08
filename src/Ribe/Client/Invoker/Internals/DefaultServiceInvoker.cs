using Ribe.Core.Service.Address;
using Ribe.Infrustructure;
using Ribe.Messaging;
using Ribe.Serialize;
using System;
using System.Threading.Tasks;

namespace Ribe.Client.Invoker.Internals
{
    public class DefaultServiceInvoker : IServiceInvoker
    {
        private IMessageFactory _messageFactory;

        private IRpcClientFacotry _clientFacotry;

        private IIdGenerator _idGenerator;

        public ServiceAddress ServiceAddress { get; internal set; }

        public DefaultServiceInvoker(
            IIdGenerator idGenerator,
            IMessageFactory messageFactory,
            IRpcClientFacotry clientFacotry
        )
        {
            _idGenerator = idGenerator;
            _messageFactory = messageFactory;
            _clientFacotry = clientFacotry;
        }

        public async Task<object> InvokeAsync(Type valueType, object[] paramterValues, RpcServiceProxyOption options)
        {
            var isAsyncCall = IsAsyncCall(valueType);
            var isVoidCall = IsVoidCall(valueType);
            var dataType = isAsyncCall && !isVoidCall ? valueType.GetGenericArguments()[0] : valueType;

            EnsureRequestId(options);

            var message = _messageFactory.Create(options, paramterValues);
            if (message == null)
            {
                throw new RpcException("create invoke message failed!", options[Constants.RequestId]);
            }

            var client = _clientFacotry.CreateClient(ServiceAddress);
            var returnMessage = await client.InvokeAsync(message);
            if (returnMessage == null)
            {
                throw new RpcException("return message is null!", options[Constants.RequestId]);
            }

            var result = returnMessage.GetResult(dataType);
            if (result == null)
            {
                throw new RpcException("return value is null", options[Constants.RequestId]);
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                throw new RpcException(result.Error, options[Constants.RequestId]);
            }

            if (!isAsyncCall)
            {
                return result.Data;
            }

            return Task.FromResult(result.Data);
        }

        private void EnsureRequestId(RpcServiceProxyOption options)
        {
            if (!options.ContainsKey(Constants.RequestId))
            {
                options[Constants.RequestId] = _idGenerator.CreateId().ToString();
            }
        }

        private static bool IsAsyncCall(Type valueType)
        {
            return typeof(Task).IsAssignableFrom(valueType);
        }

        private static bool IsVoidCall(Type valueType)
        {
            return typeof(Task) == valueType || typeof(void) == valueType;
        }
    }
}
