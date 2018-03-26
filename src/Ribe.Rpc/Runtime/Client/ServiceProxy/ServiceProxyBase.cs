using Ribe.Rpc.Core;
using Ribe.Rpc.Core.Service;
using Ribe.Rpc.Runtime.Client.Invoker;
using System;
using System.Linq;
using System.Reflection;

namespace Ribe.Rpc.Runtime.Client.ServiceProxy
{
    /// <summary>
    /// the abstract class of service proxy 
    /// </summary>
    public abstract class ServiceProxyBase
    {
        public Header Header { get; }

        protected Type ServiceType { get; }

        protected IServicePathFacotry ServicePathFacotry { get; }

        protected IServiceInvokerProvider ServiceInvokerProvider { get; }

        internal static readonly MethodInfo InvokeServiceMethod;

        internal static readonly MethodInfo InvokeVoidServiceMethod;

        static ServiceProxyBase()
        {
            InvokeServiceMethod = typeof(ServiceProxyBase).GetMethod(nameof(InvokeService), BindingFlags.Instance | BindingFlags.NonPublic);
            InvokeVoidServiceMethod = typeof(ServiceProxyBase).GetMethod(nameof(InvokeVoidService), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public ServiceProxyBase(IServiceInvokerProvider serviceInvokerProvider, IServicePathFacotry servicePathFacotry)
        {
            Header = new Header();

            ServicePathFacotry = servicePathFacotry;
            ServiceInvokerProvider = serviceInvokerProvider;
            ServiceType = GetType().GetInterfaces().FirstOrDefault();

            if (ServiceType == null)
            {
                throw new NotSupportedException("ServiceProxy must based on an interface");
            }
        }

        protected object InvokeService(string method, Type valueType, object[] paramterValues)
        {
            var header = GetRequestHeader(method);
            var req = new Invocation(header, ServiceType, paramterValues, valueType);

            var invoker = ServiceInvokerProvider.GetInvoker(req);
            if (invoker == null)
            {
                throw new RpcException("get ServiceInvoker failed!");
            }

            return invoker.InvokeAsync(req).Result;
        }

        private Header GetRequestHeader(string method)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new NullReferenceException(nameof(method));
            }

            var header = Header.Clone();

            header[Constants.MethodName] = method;
            header[Constants.ServiceName] = ServiceType.Namespace + "." + ServiceType.Name;

            return header;
        }

        protected void InvokeVoidService(string methodKey, Type valueType, object[] paramterValues)
        {
            InvokeService(methodKey, valueType, paramterValues);
        }
    }
}