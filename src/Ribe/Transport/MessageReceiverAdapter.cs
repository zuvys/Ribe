using Ribe.Core;
using Ribe.Messaging;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Transport
{
    public class MessageReceiverAdapter : IMessageReceiver
    {
        private IMessageConvertorProvider _messageConvertorProvider;

        public MessageReceiverAdapter(IMessageConvertorProvider messageConvertorProvider)
        {
            _messageConvertorProvider = messageConvertorProvider;
        }

        public Task ReceiveAsync(Message message)
        {
            var convertor = _messageConvertorProvider.GetConvertor(message);
            if (convertor == null)
            {
                throw new NotSupportedException("not supported!");
            }

            if (message.IsRequest)
            {
                return ReceiveRequstAsync(convertor.ConvertToRequest(message));
            }

            return ReceiveResponseAsync(convertor.ConvertToResponse(message, typeof(void)));
        }

        public Task ReceiveAsync(Message message, Type valueType)
        {
            var convertor = _messageConvertorProvider.GetConvertor(message);
            if (convertor == null)
            {
                throw new NotSupportedException("not supported!");
            }

            if (message.IsRequest)
            {
                return ReceiveRequstAsync(convertor.ConvertToRequest(message));
            }

            return ReceiveResponseAsync(convertor.ConvertToResponse(message, valueType));
        }

        protected virtual Task ReceiveRequstAsync(Request request)
        {
            return Task.CompletedTask;
        }

        protected virtual Task ReceiveResponseAsync(Response response)
        {
            return Task.CompletedTask;
        }
    }
}
