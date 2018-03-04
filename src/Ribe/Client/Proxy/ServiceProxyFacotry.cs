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
using System.Threading.Tasks;

namespace Ribe.Client.Proxy
{
    public class ServiceProxyFacotry : IServiceProxyFactory
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

        static ServiceProxyFacotry()
        {
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(ModuleName);
            ServiceProxies = new ConcurrentDictionary<Type, Type>();
        }

        public ServiceProxyFacotry(ISerializer serializer, IMessageFactory messageFactory)
        {
            //Inject
            /*IClientFactory,IMessageFactory*/
            _serializer = serializer;
            _messageFactory = messageFactory;
            _serviceMethodKeyFactory = new DefaultServiceMethodKeyFactory(new LoggerFactory().CreateLogger("WWW"));
        }

        public TService CreateProxy<TService>(Func<ServiceProxyOption> builder = null)
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
                    null,
                    new[] { serviceType });

                var serializerField = typeBudiler.DefineField("_serializer", typeof(ISerializer), FieldAttributes.Private);
                var messageFacotryField = typeBudiler.DefineField("_messageFacotry", typeof(IMessageFactory), FieldAttributes.Private);
                var optionsField = typeBudiler.DefineField("_options", typeof(ServiceProxyOption), FieldAttributes.Private);
                var pathField = typeBudiler.DefineField("_path", typeof(string), FieldAttributes.Private);

                var ctorBudiler = typeBudiler.DefineConstructor(
                    MethodAttributes.Public,
                    CallingConventions.HasThis,
                    new[] { typeof(ISerializer), typeof(IMessageFactory), typeof(ServiceProxyOption), typeof(string) });

                var il = ctorBudiler.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, serializerField);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Stfld, messageFacotryField);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_3);
                il.Emit(OpCodes.Stfld, optionsField);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg, 4);
                il.Emit(OpCodes.Stfld, pathField);
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
                    il.Emit(OpCodes.Ldfld, pathField);
                    il.Emit(OpCodes.Ldstr, methodKey);
                    il.Emit(OpCodes.Ldtoken, item.ReturnType);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, optionsField);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, serializerField);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, messageFacotryField);
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

                    il.Emit(OpCodes.Call, typeof(ServiceProxyFacotry).GetMethod(nameof(InvokeAsync)));
                    il.Emit(OpCodes.Ret);
                }

                return typeBudiler.CreateType();
            });
            var options = builder != null ? builder() : new ServiceProxyOption();
            //TODO:Client Server 两端调用相同生成器
            var path = $"/{ options[Constants.Group]}/{ type.Namespace + "." + type.Name}/{options[Constants.Version]}/";

            return (TService)Activator.CreateInstance(proxy, _serializer, _messageFactory, options, path);
        }

        public static object InvokeAsync(
            string path,
            string methodKey,
            Type returnType,
            ServiceProxyOption options,
            ISerializer serializer,
            IMessageFactory messageFactory,
            object[] paramterValues)
        {
            options[Constants.RequestId] = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString();
            options[Constants.ServicePath] = path;
            options[Constants.ServiceMethodKey] = methodKey;

            var body = serializer.SerializeObject(paramterValues);
            var message = messageFactory.Create(options, body);

            var client = new NettyClient();

            client.ConnectAsync(new Core.Service.Address.ServiceAddress()
            {
                Ip = "127.0.0.1",
                Port = 8080
            }).Wait();

            if (typeof(Task).IsAssignableFrom(returnType))
            {
                if (typeof(Task) == returnType)
                {
                    return Task.CompletedTask;
                }

                return Task.FromResult(client.InvokeAsync(message).Result.GetResult(returnType.GetGenericArguments()[0]).Data);
            }

            return client.InvokeAsync(message).Result.GetResult(returnType).Data;
        }
    }
}
