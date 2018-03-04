using Ribe.Client.Proxy;
using Ribe.Json.Messaging;
using Ribe.Json.Serialize;
using ServiceInterfaces;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProxy = new ServiceProxyFacotry(
                new JsonSerializer(),
                new JsonMessageFactory()
                );

            var proxy = serviceProxy.CreateProxy<IShopService>();
            var goods = proxy.GetGoods(2);

            Console.WriteLine("GoodsName:" + goods.Name);
            Console.ReadLine();
        }
    }
}
