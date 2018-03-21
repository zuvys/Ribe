using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Codecs;
using Ribe.Core.Service;
using Ribe.Core.Service.Address;
using Ribe.DotNetty.Adapter;
using Ribe.Messaging;
using Ribe.Rpc.Server;
using Ribe.Serialize;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Host
{
    public class DotNettyServer : RpcServer
    {
        private ServiceEntryCache _cache;

        private IEncoderProvider _encoderProvider;

        private IDecoderProvider _decoderProvider;

        private ISerializerProvider _serializerProvider;

        private MultithreadEventLoopGroup _bossGroup;

        private MultithreadEventLoopGroup _workerGroup;

        public DotNettyServer(
            ServiceEntryCache cahche,
            IEncoderProvider encoderProvider,
            IDecoderProvider decoderProvider,
            IMessageConvertorProvider messageConvertorProvider,
            ISerializerProvider serializerProvider,
            ServiceAddress address
        ) : base(address)
        {
            _cache = cahche;
            _encoderProvider = encoderProvider;
            _decoderProvider = decoderProvider;
            _serializerProvider = serializerProvider;

            _bossGroup = new MultithreadEventLoopGroup(1);
            _workerGroup = new MultithreadEventLoopGroup();
        }

        public override async Task StartAsync()
        {
            await new ServerBootstrap()
                  .Group(_bossGroup, _workerGroup)
                  .Channel<TcpServerSocketChannel>()
                  .Option(ChannelOption.SoBacklog, 100)
                  .Handler(new LoggingHandler("SRV-LSTN"))
                  .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                  {
                      channel.Pipeline.AddLast(new LengthFieldPrepender(4));
                      channel.Pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                      channel.Pipeline.AddLast(new DotNettyChannelDecoderHandlerAdapter(_decoderProvider));
                      channel.Pipeline.AddLast(new DotNettyChannelEncoderHandlerAdapter(_encoderProvider));
                      channel.Pipeline.AddLast(new DotNettyChannelServerHandlerAdapter(null, _serializerProvider));
                  })).BindAsync(Port);
        }

        public override async Task StopAsync()
        {
            if (_bossGroup != null)
            {
                await _bossGroup.ShutdownGracefullyAsync();
            }

            if (_workerGroup != null)
            {
                await _workerGroup.ShutdownGracefullyAsync();
            }
        }
    }
}
