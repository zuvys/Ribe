using Microsoft.Extensions.Logging;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty.Client;
using Ribe.Messaging;
using Ribe.Serialize;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Ribe.Client.ServiceProxy
{
    public class DefaultServiceProxyFactory : IServiceProxyFactory
    {
        const string AssemblyName = "Ribe_Client_Proxy";

        const string ModuleName = "Ribe_Client_Proxy_Module";

        const string ProxyTypePrefix = "Ribe_Client_Proxy_";

        internal static AssemblyBuilder AssemblyBuilder { get; }

        internal static ModuleBuilder ModuleBuilder { get; }

        private static ConcurrentDictionary<Type, Type> ServiceProxies { get; }

        private IServiceMethodKeyFactory _serviceMethodKeyFactory;

        private ISerializer _serializer;

        private IMessageFactory _messageFactory;

        static DefaultServiceProxyFactory()
        {
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(ModuleName);
            ServiceProxies = new ConcurrentDictionary<Type, Type>();
        }

        public DefaultServiceProxyFactory(ISerializer serializer, IMessageFactory messageFactory)
        {
            _serializer = serializer;
            _messageFactory = messageFactory;
            _serviceMethodKeyFactory = new DefaultServiceMethodKeyFactory(new LoggerFactory().CreateLogger("WWW"));
        }

        public TService CreateProxy<TService>(Func<RpcServiceProxyOption> builder = null)
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

                var ctorTypes = new[] { typeof(ISerializer), typeof(IMessageFactory), typeof(RpcServiceProxyOption) };
                var ctorBudiler = typeBudiler.DefineConstructor(
                    MethodAttributes.Public,
                    CallingConventions.HasThis,
                    ctorTypes);

                var il = ctorBudiler.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Ldarg_3);
                il.Emit(OpCodes.Call, typeof(ServiceProxyBase).GetConstructor(ctorTypes));
                il.Emit(OpCodes.Ret);

                foreach (var item in serviceType.GetMethods())
                {
                    var paramterTypes = item.GetParameters().Select(i => i.ParameterType).ToArray();
                    var methodBudiler = typeBudiler.DefineMethod(
                           item.Name,
                           item.Attributes & (~MethodAttributes.Abstract),
                           item.ReturnType,
                           paramterTypes);

                    var methodKey = _serviceMethodKeyFactory.CreateMethodKey(item);

                    il = methodBudiler.GetILGenerator();

                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldstr, methodKey);
                    il.Emit(OpCodes.Ldtoken, item.ReturnType);
                    il.Emit(OpCodes.Ldc_I4, item.GetParameters().Length);
                    il.Emit(OpCodes.Newarr, typeof(object));

                    for (var i = 0; i < item.GetParameters().Length; i++)
                    {
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldc_I4, i);
                        il.Emit(OpCodes.Ldarg, i + 1);

                        if (item.GetParameters()[i].ParameterType.IsValueType)
                        {
                            il.Emit(OpCodes.Box, item.GetParameters()[i].ParameterType);
                        }

                        il.Emit(OpCodes.Stelem_Ref);
                    }

                    il.Emit(OpCodes.Call, ServiceProxyBase.InvokeMethod);
                    il.Emit(OpCodes.Ret);
                }

                return typeBudiler.CreateType();
            });

            var options = builder != null ? builder() : new RpcServiceProxyOption();

            options[Constants.ServicePath] = $"/{ options[Constants.Group]}/{ type.Namespace + "." + type.Name}/{options[Constants.Version]}/";
            return (TService)Activator.CreateInstance(proxy, _serializer, _messageFactory, options);
        }
    }
}
