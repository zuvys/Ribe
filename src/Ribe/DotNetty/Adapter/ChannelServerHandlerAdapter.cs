using DotNetty.Transport.Channels;
using Ribe.Core;
using Ribe.DotNetty.Messaging;
using Ribe.Messaging;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelServerHandlerAdapter : ChannelHandlerAdapter
    {
        private IServerInvokerFacotry _serviceInvokerFacotry;

        public ChannelServerHandlerAdapter(IServerInvokerFacotry serviceInvokerFacotry)
        {
            _serviceInvokerFacotry = serviceInvokerFacotry;
        }

        public async override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            var message = (Message)msg;
            if (message != null)
            {
                var ctx = message.GetInvokeContext();
                if (ctx != null)
                {
                    ctx.Response = new DotNettyMessageSender(context.Channel, true);
                }

                await _serviceInvokerFacotry.CreateInvoker(ctx).InvokeAsync();
            }
        }
    }
}
