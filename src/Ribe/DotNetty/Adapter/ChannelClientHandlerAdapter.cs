using DotNetty.Transport.Channels;
using Ribe.Core;
using Ribe.Messaging;
using System;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelClientHandlerAdapter : ChannelHandlerAdapter
    {
        private Action<IMessage> _handler;

        public ChannelClientHandlerAdapter(Action<IMessage> handler)
        {
            _handler = handler;
        }

        public override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            var message = (IMessage)msg;
            if (message != null)
            {
                _handler(message);
            }
        }
    }
}
