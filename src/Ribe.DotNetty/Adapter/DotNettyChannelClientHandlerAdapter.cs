using DotNetty.Transport.Channels;
using Ribe.Messaging;
using System;

namespace Ribe.DotNetty.Adapter
{
    public class DotNettyChannelClientHandlerAdapter : ChannelHandlerAdapter
    {
        private Action<Message> _handler;

        public DotNettyChannelClientHandlerAdapter(Action<Message> handler)
        {
            _handler = handler;
        }

        public override void ChannelRead(IChannelHandlerContext context, object obj)
        {
            if (obj is Message message)
            {
                _handler(message);
            }
        }
    }
}
