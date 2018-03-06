using Ribe.Client.Invoker;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ribe.Client.ServiceProxy
{
    public class ServiceProxyBase
    {
        internal static MethodInfo InvokeMethod = typeof(ServiceProxyBase).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic);

        protected RpcServiceProxyOption Options { get; }

        protected IServiceInvokerProvider ServiceInvokerProvider { get; }

        public ServiceProxyBase(IServiceInvokerProvider invokerProvider, RpcServiceProxyOption options)
        {
            ServiceInvokerProvider = invokerProvider;
            Options = options;
        }

        protected object Invoke(string methodKey, Type valueType, object[] paramterValues)
        {
            var options = Options.Clone(new[] { new KeyValuePair<string, string>(Constants.ServiceMethodKey, methodKey) });
            var invoker = ServiceInvokerProvider.GetInvoker();
            if (invoker == null)
            {
                //log
                throw new RpcException("can not get an IServiceInvoker!");
            }

            return invoker.InvokeAsync(valueType, paramterValues, options);
        }
    }
}