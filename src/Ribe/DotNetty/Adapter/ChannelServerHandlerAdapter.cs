using DotNetty.Transport.Channels;
using Ribe.Core;
using Ribe.DotNetty.Messaging;
using Ribe.Messaging;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelServerHandlerAdapter : ChannelHandlerAdapter
    {
        private IServerInvokerFacotry _serviceInvokerFacotry;

        private IMessageConvertorProvider _convertorProvider;

        public ChannelServerHandlerAdapter(IServerInvokerFacotry serviceInvokerFacotry, IMessageConvertorProvider convertorProvider)
        {
            _serviceInvokerFacotry = serviceInvokerFacotry;
            _convertorProvider = convertorProvider;
        }

        public async override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            var message = (Message)msg;
            if (message != null)
            {
                var convertor = _convertorProvider.GetConvertor(message);
                if (convertor == null)
                {
                    //log 
                    //response
                }

                var ctx = convertor.ConvertToServiceContext(message, null);
                if (ctx != null)
                {
                    ctx.Response = new DotNettyMessageSender(context.Channel);
                }

                await _serviceInvokerFacotry.CreateInvoker(ctx).InvokeAsync();
            }
        }
    }
}
