using Ribe.Rpc.Core.Service;
using Ribe.Rpc.Runtime.Client.Invoker;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Ribe.Rpc.Runtime.Client.ServiceProxy
{
    public class ServiceProxyFactory : IServiceProxyFactory
    {
        const string AssemblyName = "Ribe_Client_Proxy";

        const string ModuleName = "Ribe_Client_Proxy_Module";

        const string ProxyTypePrefix = "Ribe_Client_Proxy_";

        internal static AssemblyBuilder AssemblyBuilder { get; }

        internal static ModuleBuilder ModuleBuilder { get; }

        private static ConcurrentDictionary<Type, Type> ProxyCache { get; }

        private IServiceMethodNameFactory _serviceMethodNameFactory;

        private IServiceInvokerProvider _invokerProvider;

        private IServicePathFacotry _serviceNameFacotry;

        static ServiceProxyFactory()
        {
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(ModuleName);
            ProxyCache = new ConcurrentDictionary<Type, Type>();
        }

        /// <summary>
        /// ctor
        /// </summary>
        public ServiceProxyFactory(
            IServiceInvokerProvider invokerProvider,
            IServicePathFacotry serviceNameFacotry,
            IServiceMethodNameFactory serviceMethodNameFactory
        )
        {
            _invokerProvider = invokerProvider;
            _serviceNameFacotry = serviceNameFacotry;
            _serviceMethodNameFactory = serviceMethodNameFactory;
        }

        public TService CreateProxy<TService>()
        {
            var type = typeof(TService);
            if (!type.IsInterface)
            {
                throw new NotSupportedException($"type :{type.FullName} is not an interface type");
            }

            var proxy = ProxyCache.GetOrAdd(type, (serviceType) =>
            {
                var typeBudiler = ModuleBuilder.DefineType(
                    ProxyTypePrefix + "_" + serviceType.Namespace.Replace(".", "_") + "_" + serviceType.Name,
                    TypeAttributes.Class,
                    typeof(ServiceProxyBase),
                    new[] { serviceType });

                var ctorTypes = new[] { typeof(IServiceInvokerProvider), typeof(IServicePathFacotry) };
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
                        il.Emit(OpCodes.Call, ServiceProxyBase.InvokeVoidServiceMethod);
                    }
                    else
                    {
                        il.Emit(OpCodes.Call, ServiceProxyBase.InvokeServiceMethod);
                    }

                    il.Emit(OpCodes.Ret);
                }

                return typeBudiler.CreateType();
            });

            return (TService)Activator.CreateInstance(proxy, _invokerProvider, _serviceNameFacotry);
        }
    }
}
