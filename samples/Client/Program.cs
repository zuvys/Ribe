﻿using Microsoft.Extensions.Logging;
using Ribe.Client;
using Ribe.Client.Extensions;
using Ribe.Client.Invoker.Internals;
using Ribe.Client.ServiceProxy;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty.Client;
using Ribe.Json.Serialize;
using ServiceInterfaces;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new JsonSerializer();
            var clientFacotry = new DotNettyClientFactory(null);

            var serviceMethodKeyFacotry = new DefaultServiceMethodKeyFactory(
                new LoggerFactory().CreateLogger("DefaultServiceMethodKeyFactory")
            );

            var serviceInvokerProvider = new RpcInvokerProvider(
                clientFacotry
            );

            var factory = new ServiceProxyFactory(serviceInvokerProvider, serviceMethodKeyFacotry);

            var proxy = factory.CreateProxy<IShopService>(
                () => new ServiceProxyOption().WithVersion("0.0.2"));

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
