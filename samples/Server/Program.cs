using Microsoft.Extensions.Logging;
using Ribe.Codecs;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty.Host;
using Ribe.Messaging.Internal;
using Ribe.Rpc.Json.Codecs;
using Ribe.Rpc.Json.Messaging;
using Ribe.Rpc.Json.Serialize;
using Ribe.Rpc.Logging;
using Ribe.Serialize;
using ServiceInterface;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //scan
            var logger = new NullLogger();
            var facotry = new DefaultServiceEntryFactory(
                new DefaultServiceEntryPathFactory(logger),
                new DefaultServiceMethodMapFacotry(new DefaultServiceMethodKeyFactory(logger), logger),
                logger);

            var cache = new ServiceEntryCache();

            foreach (var item in facotry.CreateServices(typeof(ShopServiceImpl)))
            {
                cache.AddOrUpdate(item);
            }

            foreach (var item in facotry.CreateServices(typeof(ShopServiceImpl2)))
            {
                cache.AddOrUpdate(item);
            }

            new DotNettyServer(cache,
                new EncoderProvider(new[] { new JsonEncoder() }),
                new DecoderProvider(new[] { new JsonDecoder() }),
                new DefaultMessageConvertorProvider(new[] { new JsonMessageConvertor() }),
                new SerializerProvider(new[] { JsonSerializer.Default }),
                new Ribe.Core.Service.Address.ServiceAddress()
                {
                    Ip = "127.0.0.1",
                    Port = 8080
                }
                ).StartAsync().Wait();
            Console.ReadLine();
        }
    }
}
