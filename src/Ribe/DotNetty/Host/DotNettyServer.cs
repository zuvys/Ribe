using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using Ribe.Codecs;
using Ribe.Core.Service;
using Ribe.DotNetty.Adapter;
using Ribe.Host;
using Ribe.Messaging;
using Ribe.Messaging.Internal;
using Ribe.Serialize;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Host
{
    public class DotNettyServer : IServer
    {
        private IChannel _channel;

        private ServiceEntryCache _cache;

        private IEncoderProvider _encoderProvider;

        private IDecoderProvider _decoderProvider;

        private ISerializerProvider _serializerProvider;

        private IMessageConvertorProvider _messageConvertorProvider;

        public DotNettyServer(
            ServiceEntryCache cahche,
            IEncoderProvider encoderProvider,
            IDecoderProvider decoderProvider,
            IMessageConvertorProvider messageConvertorProvider,
            ISerializerProvider serializerProvider)
        {
            _cache = cahche;
            _encoderProvider = encoderProvider;
            _decoderProvider = decoderProvider;
            _messageConvertorProvider = messageConvertorProvider;
            _serializerProvider = serializerProvider;
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
                        pipe.AddLast(new ChannelDecoderAdapter(_decoderProvider));
                        pipe.AddLast(new ChannelEncoderAdapter(_encoderProvider));
                        pipe.AddLast(new ChannelServerHandlerAdapter(
                            _serializerProvider,
                            _messageConvertorProvider,
                            new LoggerFactory().CreateLogger<ChannelServerHandlerAdapter>()));
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
