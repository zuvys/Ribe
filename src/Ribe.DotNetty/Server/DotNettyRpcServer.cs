using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Codecs;
using Ribe.Core.Service;
using Ribe.DotNetty.Adapter;
using Ribe.Messaging;
using Ribe.Serialize;
using Ribe.Server;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Host
{
    public class DotNettyRpcServer : IRpcServer
    {
        private IChannel _channel;

        private ServiceEntryCache _cache;

        private IEncoderProvider _encoderProvider;

        private IDecoderProvider _decoderProvider;

        private ISerializerProvider _serializerProvider;

        public DotNettyRpcServer(
            ServiceEntryCache cahche,
            IEncoderProvider encoderProvider,
            IDecoderProvider decoderProvider,
            IMessageConvertorProvider messageConvertorProvider,
            ISerializerProvider serializerProvider)
        {
            _cache = cahche;
            _encoderProvider = encoderProvider;
            _decoderProvider = decoderProvider;
            _serializerProvider = serializerProvider;
        }

        public async Task StartAsync(int port)
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
                        channel.Pipeline.AddLast(new LengthFieldPrepender(4));
                        channel.Pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                        channel.Pipeline.AddLast(new DotNettyChannelDecoderHandler(_decoderProvider));
                        channel.Pipeline.AddLast(new DotNettyChannelEncoderHandler(_encoderProvider));
                        channel.Pipeline.AddLast(new DotNettyChannelServerHandler(null, _serializerProvider));
                    }));

                _channel = await bootstrap.BindAsync(port);
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
