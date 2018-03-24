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
        internal static MethodInfo InvokeServiceMethod { get; }

        internal static MethodInfo InvokeServiceVoidMethod { get; }

        protected RequestHeader Options { get; }

        protected IServiceInvokerProvider InvokderProvider { get; }

        static ServiceProxyBase()
        {
            InvokeServiceMethod = typeof(ServiceProxyBase).GetMethod(nameof(InvokeService), BindingFlags.Instance | BindingFlags.NonPublic);
            InvokeServiceVoidMethod = typeof(ServiceProxyBase).GetMethod(nameof(InvokeVoidService), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public ServiceProxyBase(IServiceInvokerProvider invokderProvider, RequestHeader options)
        {
            InvokderProvider = invokderProvider;
            Options = options;
        }

        protected object InvokeService(string methodName, Type valueType, object[] paramterValues)
        {
            var options = Options.Clone(new[] { new KeyValuePair<string, string>(Constants.ServiceMethodName, methodName) });
            var invoker = InvokderProvider.GetInvoker();
            if (invoker == null)
            {
                throw new RpcException("get ServiceInvoker failed!");
            }

            return invoker.InvokeAsync(valueType, paramterValues, options).Result;
        }

        protected void InvokeVoidService(string methodKey, Type valueType, object[] paramterValues)
        {
            InvokeService(methodKey, valueType, paramterValues);
        }
    }
}