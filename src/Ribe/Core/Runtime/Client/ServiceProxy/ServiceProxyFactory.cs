using Ribe.Core;
using Ribe.Core.Service;

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Ribe.Rpc.Core.Runtime.Client.ServiceProxy
{
    public class ServiceProxyFactory : IServiceProxyFactory
    {
        const string AssemblyName = "Ribe_Client_Proxy";

        const string ModuleName = "Ribe_Client_Proxy_Module";

        const string ProxyTypePrefix = "Ribe_Client_Proxy_";

        internal static AssemblyBuilder AssemblyBuilder { get; }

        internal static ModuleBuilder ModuleBuilder { get; }

        private static ConcurrentDictionary<Type, Type> ServiceProxies { get; }

        private IServiceMethodNameFactory _serviceMethodNameFactory;

        private Invoker.IServiceInvokerProvider _requestHandlerProvider;

        static ServiceProxyFactory()
        {
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(ModuleName);
            ServiceProxies = new ConcurrentDictionary<Type, Type>();
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceInvokerProvider"></param>
        /// <param name="serviceMethodKeyFactory"></param>
        public ServiceProxyFactory(
            Invoker.IServiceInvokerProvider requestHandlerProvider,
            IServiceMethodNameFactory serviceMethodKeyFactory
        )
        {
            _requestHandlerProvider = requestHandlerProvider;
            _serviceMethodNameFactory = serviceMethodKeyFactory;
        }

        public TService CreateProxy<TService>(Func<RequestHeader> builder = null)
        {
            var type = typeof(TService);
            if (!type.IsInterface)
            {
                throw new NotSupportedException($"type :{type.FullName} is not an interface type");
            }

            var proxy = ServiceProxies.GetOrAdd(type, (serviceType) =>
            {
                var typeBudiler = ModuleBuilder.DefineType(
                    ProxyTypePrefix + "_" + serviceType.Namespace.Replace(".", "_") + "_" + serviceType.Name,
                    TypeAttributes.Class,
                    typeof(ServiceProxyBase),
                    new[] { serviceType });

                var ctorTypes = new[] { typeof(Invoker.IServiceInvokerProvider), typeof(RequestHeader) };
                var ctorBudiler = typeBudiler.DefineConstructor(
                    MethodAttributes.Public,
                    CallingConventions.HasThis,
                    ctorTypes);

                var il = ctorBudiler.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Call, typeof(ServiceProxyBase).GetConstructor(ctorTypes));
                il.Emit(OpCodes.Ret);

                foreach (var item in serviceType.GetMethods())
                {
                    var methodName = _serviceMethodNameFactory.CreateName(item);
                    var paramterInfos = item.GetParameters();
                    var paramterTypes = paramterInfos.Select(i => i.ParameterType).ToArray();
                    var methodBudiler = typeBudiler.DefineMethod(
                           item.Name,
                           item.Attributes & (~MethodAttributes.Abstract),
                           item.ReturnType,
                           paramterTypes);

                    il = methodBudiler.GetILGenerator();

                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldstr, methodName);
                    il.Emit(OpCodes.Ldtoken, item.ReturnType);
                    il.Emit(OpCodes.Ldc_I4, paramterTypes.Length);
                    il.Emit(OpCodes.Newarr, typeof(object));

                    for (var i = 0; i < paramterInfos.Length; i++)
                    {
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldc_I4, i);
                        il.Emit(OpCodes.Ldarg, i + 1);

                        if (paramterInfos[i].ParameterType.IsValueType)
                        {
                            il.Emit(OpCodes.Box, paramterInfos[i].ParameterType);
                            il.Emit(OpCodes.Stelem_Ref);
                        }
                    }

                    if (item.ReturnType == typeof(void))
                    {
                        il.Emit(OpCodes.Call, ServiceProxyBase.InvokeServiceVoidMethod);
                    }
                    else
                    {
                        il.Emit(OpCodes.Call, ServiceProxyBase.InvokeServiceMethod);
                    }

                    il.Emit(OpCodes.Ret);
                }

                return typeBudiler.CreateType();
            });

            var options = builder != null ? builder() : new RequestHeader();

            //TODO:ServicePath在服务端生成
            options[Constants.ServicePath] = $"/{ options[Constants.Group]}/{ type.Namespace + "." + type.Name}/{options[Constants.Version]}/";
            return (TService)Activator.CreateInstance(proxy, _requestHandlerProvider, options);
        }
    }
}
