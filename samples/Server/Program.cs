using Microsoft.Extensions.DependencyInjection;
using Ribe.Codecs;
using Ribe.Core;
using Ribe.Core.Executor;
using Ribe.Core.Executor.Internals;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty;
using Ribe.Messaging;
using Ribe.Rpc;
using Ribe.Rpc.Core.Runtime.Server;
using Ribe.Rpc.DotNetty.Core.Runtime.Server;
using Ribe.Rpc.Json;
using Ribe.Rpc.Json.Codecs;
using Ribe.Rpc.Json.Messaging;
using Ribe.Rpc.Json.Serialize;
using Ribe.Rpc.Logging;
using Ribe.Rpc.Server;
using Ribe.Rpc.Zookeeper;
using Ribe.Serialize;
using ServiceInterface;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new RpcServerBuilder((p) => p.UseJson())
                .AddRibeCore()
                .AddDotNetty()
                .AddZookpeer(() => new ZkConfiguration()
            {
                Address = "127.0.0.1:2181",
                RootPath = "/services/test",
                SessionTimeout = 1000 * 60
            }).BuildServiceHostFacotry();

            factory.Create(8080).StartAsync().Wait();
            factory.Create(8081).StartAsync().Wait();

            Console.ReadLine();
        }
    }
}
