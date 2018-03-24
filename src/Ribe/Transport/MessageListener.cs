using Ribe.Core;
using Ribe.Messaging;
using Ribe.Rpc.Transport;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Server
{
    public class MessageListener : IMessageListener
    {
        private IRequestHandler _handler;

        private IMessageFormatterProvider _messageConvertorProvider;

        public MessageListener(
            IRequestHandler handler,
            IMessageFormatterProvider messageConvertorProvider
        )
        {
            _handler = handler;
            _messageConvertorProvider = messageConvertorProvider;
        }

        public Task ReceiveAsync(Message message, Func<long, Response, Task> onCompleted)
        {
            var convertor = _messageConvertorProvider.GetFormatter(message);
            if (convertor == null)
            {
                throw new NotSupportedException("not supported!");
            }

            return _handler.HandleRequestAsync(convertor.FormatRequest(message), onCompleted);
        }
    }
}
