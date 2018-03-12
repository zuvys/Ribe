using Ribe.Core.Service.Address;
using Ribe.Messaging;
using System;
using System.Threading.Tasks;

namespace Ribe.Client.Invoker.Internals
{
    /// <summary>
    /// default <see cref="IRemoteServiceInvoker"/> 
    /// </summary>
    public class DefaultRemoteServiceInvoker : IRemoteServiceInvoker
    {
        private IRpcClientFacotry _clientFacotry;

        private IMessageConvertorProvider _convertorProvider;

        public ServiceAddress ServiceAddress { get; internal set; }

        public DefaultRemoteServiceInvoker(IRpcClientFacotry clientFacotry, IMessageConvertorProvider convetorProvider)
        {
            _clientFacotry = clientFacotry;
            _convertorProvider = convetorProvider;
        }

        /// <summary>
        /// Invoke Service
        /// </summary>
        /// <param name="valueType"></param>
        /// <param name="paramterValues"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<object> InvokeAsync(Type valueType, object[] paramterValues, ServiceProxyOption options)
        {
            var isVoid = IsVoidCall(valueType);
            var isAsync = IsAsyncCall(valueType);
            var dataType = isAsync && !isVoid ? valueType.GetGenericArguments()[0] : valueType;

            using (var client = _clientFacotry.CreateClient(ServiceAddress))
            {
                var id = await client.SendRequestAsync(new RemoteCallMessage(options, paramterValues));

                var message = await client.GetReponseAsync(id);
                if (message == null)
                {
                    throw new NullReferenceException(nameof(message));
                }

                var convertor = _convertorProvider.GetConvertor(message);
                if (convertor == null)
                {
                    throw new NotSupportedException("not supported!");
                }

                var data = convertor.ConvertToResult(message, dataType);
                if (data == null)
                {
                    throw new RpcServerException("return value is null", id);
                }

                if (!string.IsNullOrEmpty(data.Error))
                {
                    throw new RpcServerException(data.Error, id);
                }

                if (isVoid)
                {
                    return isAsync ? Task.CompletedTask : null;
                }

                return isAsync ? Task.FromResult(data.Data) : data.Data;
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
