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

            var factory = rpc
                .AddRibeCore()
                .AddDotNetty()
                .BuildServiceHostFacotry();


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

            factory.Create(8080).StartAsync().Wait();

            Console.WriteLine("Port:" + 8080);

            Console.ReadLine();
        }
    }
}
