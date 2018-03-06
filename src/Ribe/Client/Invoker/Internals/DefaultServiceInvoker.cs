using Ribe.Core.Service.Address;
using Ribe.Messaging;
using Ribe.Serialize;
using System;
using System.Threading.Tasks;

namespace Ribe.Client.Invoker.Internals
{
    public class DefaultServiceInvoker : IServiceInvoker
    {
        private ISerializer _serializer;

        private IMessageFactory _messageFactory;

        private IRpcClientFacotry _clientFacotry;

        public DefaultServiceInvoker(
            ISerializer serializer,
            IMessageFactory messageFactory,
            IRpcClientFacotry clientFacotry
        )
        {
            _serializer = serializer;
            _messageFactory = messageFactory;
            _clientFacotry = clientFacotry;
        }

        public object InvokeAsync( Type valueType, object[] paramterValues,RpcServiceProxyOption options)
        {
            var paramterValueBytes = _serializer.SerializeObject(paramterValues);
            var invokeMessage = _messageFactory.Create(options, paramterValueBytes);

            options[Constants.RequestId] = DateTime.Now.Millisecond.ToString();

            //TODO:Hard coded for run
            var client = _clientFacotry.CreateClient(new ServiceAddress()
            {
                Ip = "127.0.0.1",
                Port = 8080
            });

            if (typeof(Task).IsAssignableFrom(valueType))
            {
                if (typeof(Task) == valueType)
                {
                    return Task.FromResult<object>(null);
                }

                var result = client.InvokeAsync(invokeMessage).Result.GetResult(valueType.GetGenericArguments()[0]);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    throw new Exception(result.Error);
                }

                return Task.FromResult(result.Data);
            }
            else
            {
                var result = client.InvokeAsync(invokeMessage).Result.GetResult(valueType);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    throw new Exception(result.Error);
                }

                return result.Data;
            }
        }
    }
}
