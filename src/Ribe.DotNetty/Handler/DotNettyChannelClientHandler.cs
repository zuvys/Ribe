using DotNetty.Transport.Channels;
using Ribe.Messaging;
using System;

namespace Ribe.DotNetty.Adapter
{
    public class DotNettyChannelClientHandler : ChannelHandlerAdapter
    {
        private Action<Message> _handler;

        public DotNettyChannelClientHandler(Action<Message> handler)
        {
            _handler = handler;
        }

        public override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            var message = (Message)msg;
            if (message != null)
            {
                _handler(message);
            }
        }
    }
}
