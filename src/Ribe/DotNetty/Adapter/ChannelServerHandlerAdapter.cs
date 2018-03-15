using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Ribe.Core;
using Ribe.Core.Executor;
using Ribe.DotNetty.Messaging;
using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelServerHandlerAdapter : ChannelHandlerAdapter
    {
        private IMessageConvertorProvider _messageConvertorProvider;

        private ILogger<ChannelClientHandlerAdapter> _logger;

        private ISerializerProvider _serializerProvider;

        public ChannelServerHandlerAdapter(
            ISerializerProvider serializerProvider,
            IMessageConvertorProvider messageConvertorProvider,
            ILogger<ChannelClientHandlerAdapter> logger
        )
        {
            _logger = logger;
            _serializerProvider = serializerProvider;
            _messageConvertorProvider = messageConvertorProvider;
        }

        public async override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            var message = (Message)msg;
            if (message != null)
            {
                var convertor = _messageConvertorProvider.GetConvertor(message);
                var sender = new DotNettyResponseMessageSender(context.Channel, _serializerProvider);

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

                var request = convertor.ConvertToRequest(message);
            }
        }
    }
}
