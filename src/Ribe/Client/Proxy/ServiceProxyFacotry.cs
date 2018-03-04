using Microsoft.Extensions.Logging;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty.Client;
using Ribe.Messaging;
using Ribe.Serialize;
using System;
using System.Collections.Generic;
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

        private IServiceMethodKeyFactory _serviceMethodKeyFactory;

        private ISerializer _serializer;

        private IMessageFactory _messageFactory;

        static ServiceProxyFacotry()
        {
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(ModuleName);
        }

        public ServiceProxyFacotry(ISerializer serializer, IMessageFactory messageFactory)
        {
            //Inject
            /*IClientFactory,IMessageFactory*/
            _serializer = serializer;
            _messageFactory = messageFactory;
            _serviceMethodKeyFactory = new DefaultServiceMethodKeyFactory(new LoggerFactory().CreateLogger("WWW"));
        }

        public TService CreateProxy<TService>()
        {
            var serviceType = typeof(TService);
            if (!serviceType.IsInterface)
            {
                throw new NotSupportedException($"type :{serviceType.FullName} is not an interface type");
            }

            var methods = serviceType.GetMethods();

            var typeBudiler = ModuleBuilder.DefineType(
                ProxyTypePrefix + "_" + serviceType.Namespace.Replace(".", "_") + "_" + serviceType.Name,
                TypeAttributes.Class,
                null,
                new[] { serviceType });

            var serializerBuilder = typeBudiler.DefineField("_serializer", typeof(ISerializer), FieldAttributes.Private);
            var messageFacotryBuilder = typeBudiler.DefineField("_messageFacotry", typeof(IMessageFactory), FieldAttributes.Private);

            var ctorBudiler = typeBudiler.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.HasThis,
                new[] { typeof(ISerializer), typeof(IMessageFactory) });

            var il = ctorBudiler.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, serializerBuilder);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, messageFacotryBuilder);
            il.Emit(OpCodes.Ret);

            foreach (var item in methods)
            {
                var paramterTypes = item.GetParameters().Select(i => i.ParameterType).ToArray();
                var methodBudiler = typeBudiler.DefineMethod(
                       item.Name,
                       item.Attributes & (~MethodAttributes.Abstract),
                       item.ReturnType,
                       paramterTypes);

                var path = "/default/" + serviceType.Namespace + "." + serviceType.Name + "/0.0.1/";
                var methodKey = _serviceMethodKeyFactory.CreateMethodKey(item);

                il = methodBudiler.GetILGenerator();

                il.Emit(OpCodes.Ldstr, path);
                il.Emit(OpCodes.Ldstr, methodKey);
                il.Emit(OpCodes.Ldtoken, item.ReturnType);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, serializerBuilder);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, messageFacotryBuilder);
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

            return (TService)Activator.CreateInstance(typeBudiler.CreateType(), _serializer, _messageFactory);
        }

        public static object InvokeAsync2(
          object[] paramterValues)
        {
            return paramterValues.Length;
        }

        public static object InvokeAsync(
            string path,
            string methodKey,
            Type resultValueType,
            ISerializer serializer,
            IMessageFactory messageFactory,
            object[] paramterValues)
        {
            var headers = new Dictionary<string, string>()
            {
                [Constants.RequestId] = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString(),
                [Constants.ServicePath] = path,
                [Constants.ServiceMethodKey] = methodKey
            };

            var body = serializer.SerializeObject(paramterValues);
            var message = messageFactory.Create(headers, body);

            var client = new NettyClient();

            client.ConnectAsync(new Core.Service.Address.ServiceAddress()
            {
                Ip = "127.0.0.1",
                Port = 8080
            }).Wait();

            var dto = client.InvokeAsync(message).Result.GetResult(resultValueType);

            if (!string.IsNullOrWhiteSpace(dto.Error))
            {
                throw new Exception(dto.Error);
            }

            return dto.Result;
        }
    }
}
