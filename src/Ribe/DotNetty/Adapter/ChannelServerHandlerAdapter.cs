using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Ribe.Core;
using Ribe.DotNetty.Messaging;
using Ribe.Messaging;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelServerHandlerAdapter : ChannelHandlerAdapter
    {
        private IServerInvokerFacotry _serviceInvokerFacotry;

        private IMessageConvertorProvider _convertorProvider;

        private ILogger<ChannelClientHandlerAdapter> _logger;

        public ChannelServerHandlerAdapter(
            IServerInvokerFacotry serviceInvokerFacotry,
            IMessageConvertorProvider convertorProvider,
            ILogger<ChannelClientHandlerAdapter> logger)
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
                var sender = new DotNettyResponseMessageSender(context.Channel, null);

                if (convertor == null)
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                        _logger.LogDebug("the message cannot convert to ServiceContext!", message.Headers[Constants.RequestId]);

                    await sender.SendAsync(new ResponseMessage(
                            message.Headers, 
                            Result.Failed("the service invoke is failed!")
                            )
                        );
                }

                var ctx = convertor.ConvertToRequestContext(message, null);
                if (ctx != null)
                {
                    ctx.Response = new DotNettyMessageSender(context.Channel);
                }

                await _serviceInvokerFacotry.CreateInvoker(ctx).InvokeAsync();
            }
        }
    }
}
