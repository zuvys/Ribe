using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using Ribe.Core.Executor;
using Ribe.Core.Executor.Internals;
using Ribe.Core.Service;
using Ribe.Core.Service.Internals;
using Ribe.DotNetty.Adapter;
using Ribe.Host;
using Ribe.Json.Codecs;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Host
{
    public class DotNettyServer : IServer
    {
        private IChannel _channel;

        private ServiceEntryCache _cache;

        public DotNettyServer(ServiceEntryCache cahche)
        {
            _cache = cahche;
        }

        public async Task StartAsync()
        {
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();

                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipe = channel.Pipeline;

                        pipe.AddLast(new LengthFieldPrepender(4));
                        pipe.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                        pipe.AddLast(new ChannelDecoderAdapter(new JsonDecoder()));
                        pipe.AddLast(new ChannelEncoderAdapter(new JsonEncoder()));
                        pipe.AddLast(new ChannelServerHandlerAdapter(null));
                    }));

                _channel = await bootstrap.BindAsync(8080);
            }
            finally
            {

            }
        }

        public Task StopAsync()
        {
            return _channel.CloseAsync();
        }
    }
}
