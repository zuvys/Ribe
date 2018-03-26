using Microsoft.Extensions.DependencyInjection;
using Ribe.Rpc.Logging;
using Ribe.Rpc.Routing.Discovery;
using Ribe.Rpc.Routing.Registry;
using Ribe.Rpc.Zookeeper.Discovery;
using Ribe.Rpc.Zookeeper.Registry;
using Ribe.Serialize;
using System;

namespace Ribe.Rpc.Zookeeper
{
    public static class RpcBuilderZkExtension
    {
        public static IRpcServerBuilder AddZookpeer(this IRpcServerBuilder rpcBuilder, Func<ZkConfiguration> configurationBuilder)
        {
            rpcBuilder.ServiceCollection.AddSingleton<IRoutingEntryRegistrar>((p)
                => new ZkServiceRouteRegistrar(
                    configurationBuilder(),
                    p.GetRequiredService<ISerializerManager>(),
                    p.GetRequiredService<ILogger>()
                )
            );

            return rpcBuilder;
        }

        public static IRpcClientBuilder AddZookpeer(this IRpcClientBuilder rpcBuilder, Func<ZkConfiguration> configurationBuilder)
        {
            rpcBuilder.ServiceCollection.AddSingleton<IRoutingEntryProvider>((p)
                => new ZkServiceRouteProvider(
                    configurationBuilder(),
                    p.GetRequiredService<ISerializerManager>(),
                    p.GetRequiredService<ILogger>()
                )
            );

            return rpcBuilder;
        }
    }
}
