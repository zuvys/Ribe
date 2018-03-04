using Ribe.Client;
using Ribe.Client.Extensions;
using Ribe.Client.Proxy;
using Ribe.Json.Messaging;
using Ribe.Json.Serialize;
using ServiceInterfaces;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ServiceProxyFacotry(
                new JsonSerializer(),
                new JsonMessageFactory()
                );

            var proxy = factory.CreateProxy<IShopService>(
                () => new ServiceProxyOption().WithVersion("0.0.2"));

            Console.WriteLine("Begin");

            var goods = proxy.GetGoods(2);

            Console.WriteLine("Sync Invoke GoodsName:" + goods.Name);

            var awaiter = proxy.GetGoodsAsync(2).GetAwaiter();

            awaiter.OnCompleted(() =>
            {
                var goods2 = awaiter.GetResult();
                Console.WriteLine("Async Invoke GoodsName:" + goods2.Name);
            });
       
            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
