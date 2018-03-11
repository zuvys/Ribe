using DotNetty.Transport.Channels;
using Ribe.Core;
using Ribe.Messaging;
using System;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelClientHandlerAdapter : ChannelHandlerAdapter
    {
        private Action<Message> _handler;

        public ChannelClientHandlerAdapter(Action<Message> handler)
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
