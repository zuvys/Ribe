using DotNetty.Transport.Channels;
using Ribe.Rpc;
using Ribe.Rpc.DotNetty.Core.Runtime.Server;
using Ribe.Rpc.Messaging;
using Ribe.Rpc.Serialize;
using Ribe.Rpc.Transport;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Adapter
{
    public class DotNettyChannelServerHandlerAdapter : ChannelHandlerAdapter
    {
        private IMessageListener _listener;

        private ISerializerManager _serializerProvider;

        public DotNettyChannelServerHandlerAdapter(IMessageListener listener, ISerializerManager serializerProvider)
        {
            _listener = listener;
            _serializerProvider = serializerProvider;
        }

        public static long i;

        public override void ChannelRead(IChannelHandlerContext context, object obj)
        {
            if (!(obj is Message message))
            {
                return;
            }

            i++;
            if (i % 1000 == 0)
            {
                Console.WriteLine("i:" + i );
            }

            Task.Run(async () =>
            {
                var sender = new DotNettyServerMessageSender(context);
                var serializer = _serializerProvider.GetSerializer(message.Header[Constants.ContentType]);

                await _listener.ReceiveAsync(message, (id, response) =>
                {
                    return sender.SendAsync(
                        new Message(
                            message.Header,
                            serializer.SerializeObject(response)
                        )
                    );
                });
            });
        }
    }
}
