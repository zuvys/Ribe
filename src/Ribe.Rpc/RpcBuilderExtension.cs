using Microsoft.Extensions.DependencyInjection;
using Ribe.Rpc.Codecs;
using Ribe.Rpc.Core;
using Ribe.Rpc.Core.Executor;
using Ribe.Rpc.Core.Executor.Internals;
using Ribe.Rpc.Core.Service;
using Ribe.Rpc.Core.Service.Internals;
using Ribe.Rpc.Logging;
using Ribe.Rpc.Messaging.Formatting;
using Ribe.Rpc.Routing.Internal;
using Ribe.Rpc.Runtime.Client.Invoker;
using Ribe.Rpc.Runtime.Client.Routing;
using Ribe.Rpc.Runtime.Client.ServiceProxy;
using Ribe.Rpc.Serialize;
using Ribe.Rpc.Server;
using Ribe.Rpc.Transport;

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
            rpcBuilder.ServiceCollection.AddSingleton<IServiceEntryProvider, ServiceEntryProvider>();
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
            rpcBuilder.ServiceCollection.AddSingleton<IServiceInvokerProvider, ServiceInvokerProvider>();
            rpcBuilder.ServiceCollection.AddSingleton<IRoutingManager, RoutingManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IServicePathFacotry, ServicePathFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<IServiceMethodNameFactory, ServiceMethodNameFactory>();
            rpcBuilder.ServiceCollection.AddSingleton<ISerializerManager, SerializerManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IMessageFormatterManager, MessageFormatterManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IEncoderManager, EncoderManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IDecoderManager, DecoderManager>();
            rpcBuilder.ServiceCollection.AddSingleton<IRoutingEntryProvider, EmptyRoutingEntryProvider>();
            rpcBuilder.ServiceCollection.AddSingleton<ILogger>((p) => NullLogger.Instance);

            return rpcBuilder;
        }
    }
}
