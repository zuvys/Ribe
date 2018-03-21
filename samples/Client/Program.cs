using Microsoft.Extensions.Logging;
using Ribe.Client.Invoker.Internals;
using Ribe.Client.ServiceProxy;
using Ribe.Codecs;
using Ribe.Core;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty.Client;
using Ribe.Messaging.Internal;
using Ribe.Rpc.Json.Codecs;
using Ribe.Rpc.Json.Messaging;
using Ribe.Rpc.Json.Serialize;
using Ribe.Rpc.Logging;
using Ribe.Serialize;
using ServiceInterfaces;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new JsonSerializer();
          
            var clientFacotry = new DotNettyRpcClientFactory(new SerializerProvider(
                new[] { new JsonSerializer() }
                ),
                new EncoderProvider(new[] {
                   new JsonEncoder()
               }),
                new DecoderProvider(
                    new[] { new JsonDecoder() }
                    )
               );

            var serviceMethodKeyFacotry = new DefaultServiceMethodKeyFactory(new NullLogger());

            var serviceInvokerProvider = new RpcInvokerProvider(
                clientFacotry,
                new DefaultMessageConvertorProvider(
                    new[] {
                        new JsonMessageConvertor()
                    }
                    )
            );

            var factory = new ServiceProxyFactory(serviceInvokerProvider, serviceMethodKeyFacotry);

            var proxy = factory.CreateProxy<IShopService>(
                () => new RequestHeader());

            Console.WriteLine("Begin");

            try
            {
                var goods = proxy.GetGoods(2);

                Console.WriteLine("Sync Invoke GoodsName:" + goods.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
