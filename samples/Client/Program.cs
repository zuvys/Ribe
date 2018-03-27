using Ribe.DotNetty;
using Ribe.Rpc;
using Ribe.Rpc.Core.Service.Address;
using Ribe.Rpc.Json;
using Ribe.Rpc.Zookeeper;
using ServiceInterfaces;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxyFacotry = new RpcClientBuilder((p) => p.UseJson())
                .AddRibeCore()
                .AddDotNetty()
                .BuildServiceProxyFacotry();

            var proxy = proxyFacotry.CreateProxy<IShopService>(new ServiceAddress("127.0.0.1", 8080));
            var goods = proxy.GetGoodsAsync(2);

            Console.ReadLine();
        }
    }
}
