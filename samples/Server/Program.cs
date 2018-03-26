using Microsoft.Extensions.DependencyInjection;
using Ribe.DotNetty;
using Ribe.Rpc;
using Ribe.Rpc.Core.Service;
using Ribe.Rpc.Core.Service.Address;
using Ribe.Rpc.Json;
using Ribe.Rpc.Runtime.Client.Routing;
using Ribe.Rpc.Runtime.Client.Routing.Registry;
using Ribe.Rpc.Zookeeper;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpc = new RpcServerBuilder((p) => p.UseJson());
            var rnd = new Random(DateTime.Now.Millisecond);
            var port1 = rnd.Next(2000, 10000);
            var port2 = rnd.Next(2000, 10000);

            var factory = rpc
                .AddRibeCore()
                .AddDotNetty()
                .AddZookpeer(() => new ZkConfiguration()
                {
                    Address = "127.0.0.1:2181",
                    RootPath = "/services/test",
                    SessionTimeout = 1000 * 60
                }).BuildServiceHostFacotry();


            var ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            var ip = string.Empty;

            for (int i = 0; i < ipEntry.AddressList.Length; i++)
            {
                if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = ipEntry.AddressList[i].ToString();
                    break;
                }
            }

            var entries = rpc.ServiceProvider.GetRequiredService<IServiceEntryProvider>().GetAll();
            var registrar = rpc.ServiceProvider.GetRequiredService<IRoutingEntryRegistrar>();

            entries
                .ToList()
                .Select(item => new RoutingEntry()
                {
                    Address = new ServiceAddress(ip, port1),
                    ServicePath = item.ServicePath,
                    ServiceName = item.ServiceName,
                    Descriptions = item.Attribute.GetDescriptions()
                })
                .ToList()
                .ForEach(item => registrar.Register(item));

            entries
                .ToList()
                .Select(item => new RoutingEntry()
                {
                    Address = new ServiceAddress(ip, port2),
                    ServicePath = item.ServicePath,
                    ServiceName = item.ServiceName,
                    Descriptions = item.Attribute.GetDescriptions()
                })
                .ToList()
                .ForEach(item => registrar.Register(item));

            factory.Create(port1).StartAsync().Wait();
            factory.Create(port2).StartAsync().Wait();

            Console.WriteLine("Port:" + port1);
            Console.WriteLine("Port:" + port2);

            Console.ReadLine();
        }
    }
}
