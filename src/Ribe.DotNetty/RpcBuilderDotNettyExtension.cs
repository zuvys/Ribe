﻿using Microsoft.Extensions.DependencyInjection;
using Ribe.DotNetty.Client;
using Ribe.Rpc;
using Ribe.Rpc.Core.Runtime.Client;
using Ribe.Rpc.Core.Runtime.Server;
using Ribe.Rpc.DotNetty.Core.Runtime.Server;

namespace Ribe.DotNetty
{
    public static class RpcBuilderDotNettyExtension
    {
        public static IRpcServerBuilder AddDotNetty(this IRpcServerBuilder rpcBuilder)
        {
            rpcBuilder.ServiceCollection.AddSingleton<IServiceHostFactory, DotNettyServiceHostFactory>();

            return rpcBuilder;
        }

        public static IRpcClientBuilder AddDotNetty(this IRpcClientBuilder rpcBuilder)
        {
            rpcBuilder.ServiceCollection.AddSingleton<IServiceClientFacotry, DotNettyServiceClientFactory>();

            return rpcBuilder;
        }
    }
}
