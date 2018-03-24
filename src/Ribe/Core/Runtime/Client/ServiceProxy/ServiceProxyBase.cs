using Ribe.Core;
using Ribe.Rpc.Core.Runtime.Client.Invoker;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ribe.Rpc.Core.Runtime.Client.ServiceProxy
{
    /// <summary>
    /// the abstract class of service proxy 
    /// </summary>
    public abstract class ServiceProxyBase
    {
        internal static MethodInfo RemoteCallMethod { get; }

        protected RequestHeader Options { get; }

        protected IServiceInvokerProvider InvokderProvider { get; }

        static ServiceProxyBase()
        {
            RemoteCallMethod = typeof(ServiceProxyBase).GetMethod(nameof(InvokeService), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public ServiceProxyBase(IServiceInvokerProvider invokderProvider, RequestHeader options)
        {
            InvokderProvider = invokderProvider;
            Options = options;
        }

        protected object InvokeService(string methodKey, Type valueType, object[] paramterValues)
        {
            var options = Options.Clone(new[] { new KeyValuePair<string, string>(Constants.ServiceMethodName, methodKey) });
            var handler = InvokderProvider.GetInvoker();
            if (handler == null)
            {
                throw new RpcException("get RequestHandler failed!");
            }

            return handler.InvokeAsync(valueType, paramterValues, options).Result;
        }
    }
}