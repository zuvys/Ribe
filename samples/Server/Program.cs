using Ribe.Codecs;
using Ribe.Core;
using Ribe.Core.Executor;
using Ribe.Core.Executor.Internals;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.Messaging;
using Ribe.Rpc.DotNetty.Core.Runtime.Server;
using Ribe.Rpc.Json.Codecs;
using Ribe.Rpc.Json.Messaging;
using Ribe.Rpc.Json.Serialize;
using Ribe.Rpc.Logging;
using Ribe.Rpc.Server;
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
            var facotry = new ServiceFactory(
                new ServicePathFactory(logger),
                new ServiceMethodNameMapFacotry(new ServiceMethodNameFactory(logger), logger),
                logger);

            var activator = new ServiceActivator();
            var methodExecutor = new ObjectMethodExecutorProvider();
            var executor = new ServiceExecutor(activator, methodExecutor, NullLogger.Instance);
            var entryProvider = new ServiceProvider(facotry);

            var requestHandler = new RequestHandler(executor, entryProvider);
            var messageListener = new MessageListener(requestHandler, new MessageFormatterProvider(new[] { new JsonMessageFormatter() }));

            var factory = new DotNettyServiceHostFactory(
                messageListener,
                  new EncoderProvider(new[] { new JsonEncoder() }),
                  new DecoderProvider(new[] { new JsonDecoder() }),
                  new SerializerProvider(new[] { JsonSerializer.Default })
                );

            factory.Create(8080).StartAsync().Wait();
            Console.ReadLine();
        }
    }
}
