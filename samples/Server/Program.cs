using Microsoft.Extensions.Logging;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty;
using Ribe.DotNetty.Host;
using System;
using ServiceInterface;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //scan
            var logger = new LoggerFactory().CreateLogger("What");
            var facotry = new DefaultServiceEntryFactory(
                new DefaultServiceEntryPathFactory(logger),
                new DefaultServiceMethodMapFacotry(new DefaultServiceMethodKeyFactory(logger), logger),
                logger);

            var cache = new ServiceEntryCache();

            foreach (var item in facotry.CreateServices(typeof(ShopServiceImpl)))
            {
                cache.AddOrUpdate(item);
            }

            new NettyServer(cache).StartAsync().Wait();
            Console.ReadLine();
        }
    }
}
