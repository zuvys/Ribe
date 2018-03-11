using Ribe.Core.Service.Address;
using Ribe.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ribe.Client.Invoker.Internals
{
    public class DefaultRemoteServiceInvoker : IRemoteServiceInvoker
    {
        /// <summary>
        /// RequestId
        /// </summary>
        private static long CurrentId = 0;

        private IRpcClientFacotry _clientFacotry;

        public ServiceAddress ServiceAddress { get; internal set; }

        static DefaultRemoteServiceInvoker()
        {
            CurrentId = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public DefaultRemoteServiceInvoker(IRpcClientFacotry clientFacotry)
        {
            _clientFacotry = clientFacotry;
        }

        public async Task<object> InvokeAsync(Type valueType, object[] paramterValues, ServiceProxyOption options)
        {
            var isAsyncCall = IsAsyncCall(valueType);
            var isVoidCall = IsVoidCall(valueType);
            var dataType = isAsyncCall && !isVoidCall ? valueType.GetGenericArguments()[0] : valueType;
            var invokeMessage = new RemoteCallMessage(options, paramterValues);

            EnsureRequestId(options);

            using (var client = _clientFacotry.CreateClient(ServiceAddress))
            {
                var requestId = await client.SendRequestAsync(invokeMessage);

                var response = await client.GetReponseAsync(requestId);

                ////var result = returnMessage.GetResult(dataType);
                ////if (result == null)
                ////{
                ////    throw new RpcException("return value is null", options[Constants.RequestId]);
                ////}

                //if (!string.IsNullOrEmpty(result.Error))
                //{
                //    throw new RpcException(result.Error, options[Constants.RequestId]);
                //}

                //if (!isAsyncCall)
                //{
                //    return result.Data;
                //}

                //return Task.FromResult(result.Data);
                return null;
            }
        }

        private void EnsureRequestId(ServiceProxyOption options)
        {
            if (!options.ContainsKey(Constants.RequestId))
            {
                options[Constants.RequestId] = Interlocked.Add(ref CurrentId, 1).ToString();
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
