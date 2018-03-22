using DotNetty.Transport.Channels;
using Ribe.Messaging;
using Ribe.Rpc.DotNetty;
using Ribe.Rpc.DotNetty.Core.Runtime.Server;
using Ribe.Rpc.Transport;
using Ribe.Serialize;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Adapter
{
    public class DotNettyChannelServerHandlerAdapter : ChannelHandlerAdapter
    {
        private IMessageListener _listener;

        private ISerializerProvider _serializerProvider;

        public DotNettyChannelServerHandlerAdapter(IMessageListener listener, ISerializerProvider serializerProvider)
        {
            _listener = listener;
            _serializerProvider = serializerProvider;
        }

        public override void ChannelRead(IChannelHandlerContext context, object obj)
        {
            if (!(obj is Message message))
            {
                return;
            }

            Task.Run(async () =>
            {
                var sender = new DotNettyServerMessageSender(context);
                var serializer = _serializerProvider.GetSerializer(message.Headers[Constants.ContentType]);

                await _listener.ReceiveAsync(message, (id, response) =>
                {
                    return sender.SendAsync(
                        new Message(
                            message.Headers,
                            serializer.SerializeObject(response)
                        )
                    );
                });
            });
        }
    }
}
