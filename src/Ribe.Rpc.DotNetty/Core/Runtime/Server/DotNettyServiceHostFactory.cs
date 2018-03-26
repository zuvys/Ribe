using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.DotNetty.Adapter;
using Ribe.Rpc.Codecs;
using Ribe.Rpc.Runtime.Server;
using Ribe.Rpc.Serialize;
using Ribe.Rpc.Transport;

namespace Ribe.Rpc.DotNetty.Core.Runtime.Server
{
    public class DotNettyServiceHostFactory : IServiceHostFactory
    {
        private IMessageListener _listener;

        private IEncoderManager _encoderManager;

        private IDecoderManager _decoderManager;

        private ISerializerManager _serializerManager;

        public DotNettyServiceHostFactory(
            IMessageListener listener,
            IEncoderManager encoderManager,
            IDecoderManager decoderManager,
            ISerializerManager serializerManager
        )
        {
            _listener = listener;
            _encoderManager = encoderManager;
            _decoderManager = decoderManager;
            _serializerManager = serializerManager;
        }

        public IServiceHost Create(int port)
        {
            return new DotNettyServiceHost(port, new ActionChannelInitializer<ISocketChannel>(channel =>
            {
                channel.Pipeline.AddLast(new LengthFieldPrepender(4));
                channel.Pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                channel.Pipeline.AddLast(new DotNettyChannelDecoderHandlerAdapter(_decoderManager));
                channel.Pipeline.AddLast(new DotNettyChannelEncoderHandlerAdapter(_encoderManager));
                channel.Pipeline.AddLast(new DotNettyChannelServerHandlerAdapter(_listener, _serializerManager));
            }));
        }
    }
}
