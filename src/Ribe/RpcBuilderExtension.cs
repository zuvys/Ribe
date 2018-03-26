using Microsoft.Extensions.DependencyInjection;
using Ribe.Codecs;
using Ribe.Core;
using Ribe.Core.Executor;
using Ribe.Core.Executor.Internals;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.Messaging;
using Ribe.Rpc.Core.Runtime.Client.Invoker;
using Ribe.Rpc.Core.Runtime.Client.ServiceProxy;
using Ribe.Rpc.Logging;
using Ribe.Rpc.Routing;
using Ribe.Rpc.Server;
using Ribe.Rpc.Transport;
using Ribe.Serialize;

namespace Ribe.Rpc
{
    public static class RpcBuilderExtension
    {
        public static IRpcServerBuilder AddRibeCore(this IRpcServerBuilder rpcBuilder)
        {
            rpcBuilder.ServiceCollection.AddSingleton<IServicePathFacotry, ServicePathFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceMethodNameFactory, ServiceMethodNameFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceMethodNameMapFactory, ServiceMethodNameMapFacotry>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceActivator, ServiceActivator>();
            rpcBuilder.ServiceCollection.AddSingleton<IRequestHandler, RequestHandler>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceFactory, ServiceFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceEntryProvider, Ribe.Core.Service.Internals.ServiceEntryProvider>();
            rpcBuilder.ServiceCollection.AddSingleton<IObjectMethodExecutorProvider, ObjectMethodExecutorProvider>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceExecutor, ServiceExecutor>();
            rpcBuilder.ServiceCollection.AddSingleton<ISerializerManager, SerializerManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IMessageFormatterManager, MessageFormatterManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IEncoderManager, EncoderManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IDecoderManager, DecoderManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IMessageListener, MessageListener>();
            rpcBuilder.ServiceCollection.AddSingleton<ILogger>((p) => NullLogger.Instance);

            return rpcBuilder;
        }

        public static IRpcClientBuilder AddRibeCore(this IRpcClientBuilder rpcBuilder)
        {
            rpcBuilder.ServiceCollection.AddSingleton<IServiceProxyFactory, ServiceProxyFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceInvokerProvider, ServiceInvokderProvider>();
            rpcBuilder.ServiceCollection.AddSingleton<IRoutingManager, RoutingManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IServicePathFacotry, ServicePathFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceMethodNameFactory, ServiceMethodNameFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<ISerializerManager, SerializerManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IMessageFormatterManager, MessageFormatterManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IEncoderManager, EncoderManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IDecoderManager, DecoderManager>();
            rpcBuilder.ServiceCollection.AddSingleton<ILogger>((p) => NullLogger.Instance);

            return rpcBuilder;
        }
    }
}
