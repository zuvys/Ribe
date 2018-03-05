using Ribe.Core.Service.Address;
using Ribe.DotNetty.Client;
using Ribe.Messaging;
using Ribe.Serialize;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Ribe.Client.ServiceProxy
{
    public class ServiceProxyBase
    {
        protected ISerializer Serializer { get; }

        protected IMessageFactory MessageFactory { get; }

        protected RpcServiceProxyOption Options { get; }

        public static MethodInfo InvokeMethod = typeof(ServiceProxyBase).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic);

        public IRpcClientFacotry ClientFacotry { get; }

        public ServiceProxyBase(
            ISerializer serializer,
            IMessageFactory messageFacotry,
            RpcServiceProxyOption options)
        {
            Serializer = serializer;
            MessageFactory = messageFacotry;
            Options = options;
            ClientFacotry = new RpcClientFactory();
        }

        protected virtual object Invoke(string methodKey, Type typeOfReturn, object[] paramterValues)
        {
            Options[Constants.RequestId] = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString();
            Options[Constants.ServiceMethodKey] = methodKey;

            var paramterValueBytes = Serializer.SerializeObject(paramterValues);
            var invokeMessage = MessageFactory.Create(Options, paramterValueBytes);

            var client = ClientFacotry.CreateClient(new ServiceAddress()
            {
                Ip = "127.0.0.1",
                Port = 8080
            });

            if (typeof(Task).IsAssignableFrom(typeOfReturn))
            {
                if (typeof(Task) == typeOfReturn)
                {
                    return Task.CompletedTask;
                }

                var result = client.InvokeAsync(invokeMessage).Result.GetResult(typeOfReturn.GetGenericArguments()[0]);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    throw new Exception(result.Error);
                }

                return Task.FromResult(result.Data);
            }
            else
            {
                var result = client.InvokeAsync(invokeMessage).Result.GetResult(typeOfReturn);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    throw new Exception(result.Error);
                }

                return result.Data;
            }
        }
    }
}