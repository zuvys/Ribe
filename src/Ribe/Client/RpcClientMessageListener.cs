using Ribe.Core;
using Ribe.Messaging;
using Ribe.Rpc.Transport;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Client
{
    public class RpcClientMessageListener : IMessageListener
    {
        private Type _responseValueType;

        private IMessageConvertorProvider _messageConvertorProvider;

        public RpcClientMessageListener(
            Type responseValueType,
            IMessageConvertorProvider messageConvertorProvider
        )
        {
            _responseValueType = responseValueType;
            _messageConvertorProvider = messageConvertorProvider;
        }

        public Task ReceiveAsync(Message message, Func<long, Response, Task> onCompleted)
        {
            var convertor = _messageConvertorProvider.GetConvertor(message);
            if (convertor == null)
            {
                throw new NotSupportedException("not supported!");
            }

            if (long.TryParse(message.Headers[Constants.RequestId], out var id))
            {
                return onCompleted(id, convertor.ConvertToResponse(message, _responseValueType));
            }

            throw new Exception("parse the request id error!");
        }
    }
}
