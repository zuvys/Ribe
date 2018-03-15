using Ribe.Client.Invoker;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ribe.Client.ServiceProxy
{
    /// <summary>
    /// the abstract class of service proxy 
    /// </summary>
    public abstract class ServiceProxyBase
    {
        internal static MethodInfo RemoteCallMethod { get; }

        protected ServiceProxyOption Options { get; }

        protected IRpcInvokerProvider RequestHandlerProvider { get; }

        static ServiceProxyBase()
        {
            RemoteCallMethod = typeof(ServiceProxyBase).GetMethod(nameof(RemoteCall), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public ServiceProxyBase(IRpcInvokerProvider requestHandlerProvider, ServiceProxyOption options)
        {
            RequestHandlerProvider = requestHandlerProvider;
            Options = options;
        }

        protected object RemoteCall(string methodKey, Type valueType, object[] paramterValues)
        {
            var options = Options.Clone(new[] { new KeyValuePair<string, string>(Constants.ServiceMethodKey, methodKey) });
            var handler = RequestHandlerProvider.GetInvoker();
            if (handler == null)
            {
                throw new RpcServerException("get RequestHandler failed!");
            }

            return handler.InvokeAsync(valueType, paramterValues, options).Result;
        }
    }
}