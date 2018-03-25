using Ribe.Core;
using Ribe.Core.Service;
using Ribe.Rpc.Core.Runtime.Client.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected IServiceInvokerProvider InvokerProvider { get; }

        protected IServiceNameFacotry ServiceNameFacotry { get; }

        public Header Header { get; }

        static ServiceProxyBase()
        {
            InvokeServiceMethod = typeof(ServiceProxyBase).GetMethod(nameof(InvokeService), BindingFlags.Instance | BindingFlags.NonPublic);
            InvokeServiceVoidMethod = typeof(ServiceProxyBase).GetMethod(nameof(InvokeVoidService), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public ServiceProxyBase(IServiceInvokerProvider invokerProvider, IServiceNameFacotry serviceNameFacotry)
        {
            Header = new Header();
            InvokerProvider = invokerProvider;
            ServiceNameFacotry = serviceNameFacotry;
        }

        protected virtual void InitializeRequestContext(RequestContext req)
        {
            var serviceType = GetType().GetInterfaces().FirstOrDefault();
            if (serviceType == null)
            {
                throw new NotSupportedException("ServiceProxy must based on an interface");
            }

            req.Header[Constants.ServiceName] = ServiceNameFacotry.CreateName(serviceType, req.Header);
        }

        protected object InvokeService(string methodName, Type valueType, object[] paramterValues)
        {
            var header = Header.Clone(new[] { new KeyValuePair<string, string>(Constants.ServiceMethodName, methodName) });
            var req = new RequestContext(header, paramterValues, valueType);

            InitializeRequestContext(req);

            var invoker = InvokerProvider.GetInvoker(req);
            if (invoker == null)
            {
                throw new RpcException("get ServiceInvoker failed!");
            }

            return invoker.InvokeAsync(req).Result;
        }

        protected void InvokeVoidService(string methodKey, Type valueType, object[] paramterValues)
        {
            InvokeService(methodKey, valueType, paramterValues);
        }
    }
}