using Ribe.Core.Service.Address;
using Ribe.Messaging;
using System;
using System.Threading.Tasks;

namespace Ribe.Client.Invoker.Internals
{
    /// <summary>
    /// default <see cref="IRpcInvoker"/> 
    /// </summary>
    public class RpcInvoker : IRpcInvoker
    {
        private IRpcClientFacotry _clientFacotry;

        private IMessageConvertorProvider _convertorProvider;

        public ServiceAddress ServiceAddress { get; internal set; }

        public RpcInvoker(IRpcClientFacotry clientFacotry, IMessageConvertorProvider convetorProvider)
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
                var message = await client.SendAsync(new RequestMessage(options, paramterValues));
                if (message == null)
                {
                    throw new NullReferenceException(nameof(message));
                }

                var convertor = _convertorProvider.GetConvertor(message);
                if (convertor == null)
                {
                    throw new NotSupportedException("not supported!");
                }

                var data = convertor.ConvertToResponse(message, dataType);
                if (data == null)
                {
                    throw new RpcServerException("return value is null");
                }

                if (!string.IsNullOrEmpty(data.Error))
                {
                    throw new RpcServerException(data.Error);
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
