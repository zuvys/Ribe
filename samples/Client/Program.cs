using Ribe.DotNetty;
using Ribe.Rpc;
using Ribe.Rpc.Json;
using Ribe.Rpc.Zookeeper;
using ServiceInterfaces;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxyFacotry = new RpcClientBuilder((p) => p.UseJson())
                .AddRibeCore()
                .AddDotNetty()
                .AddZookpeer(() => new ZkConfiguration()
                {
                    Address = "127.0.0.1:2181",
                    RootPath = "/services/test",
                    SessionTimeout = 1000 * 60,
                }).BuildServiceProxyFacotry();

            var proxy = proxyFacotry.CreateProxy<IShopService>();

            while (true)
            {
                proxy.Get(2);
                System.Threading.Thread.Sleep(1000);
            }

            Console.ReadLine();
        }
    }
}
