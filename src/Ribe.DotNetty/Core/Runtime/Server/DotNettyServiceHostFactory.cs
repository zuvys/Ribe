using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Codecs;
using Ribe.DotNetty.Adapter;
using Ribe.Rpc.Core.Runtime.Server;
using Ribe.Serialize;

namespace Ribe.Rpc.DotNetty.Core.Runtime.Server
{
    public class DotNettyServiceHostFactory : IServiceHostFactory
    {
        private IEncoderProvider _encoderProvider;

        private IDecoderProvider _decoderProvider;

        private ISerializerProvider _serializerProvider;

        public DotNettyServiceHostFactory(
            IEncoderProvider encoderProvider,
            IDecoderProvider decoderProvider,
            ISerializerProvider serializerProvider
        )
        {
            _encoderProvider = encoderProvider;
            _decoderProvider = decoderProvider;
            _serializerProvider = serializerProvider;
        }

        public IServiceHost Create(int port)
        {
            return new DotNettyServiceHost(port, new ActionChannelInitializer<ISocketChannel>(channel =>
            {
                channel.Pipeline.AddLast(new LengthFieldPrepender(4));
                channel.Pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                channel.Pipeline.AddLast(new DotNettyChannelDecoderHandlerAdapter(_decoderProvider));
                channel.Pipeline.AddLast(new DotNettyChannelEncoderHandlerAdapter(_encoderProvider));
                channel.Pipeline.AddLast(new DotNettyChannelServerHandlerAdapter(null, _serializerProvider));
            }));
        }
    }
}
