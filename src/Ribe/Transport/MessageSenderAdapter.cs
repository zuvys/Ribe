using Ribe.Messaging;
using Ribe.Serialize;

using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Transport
{
    public abstract class MessageSenderAdapter : IMessageSender
    {
        private ISerializerProvider _serializerProvider;

        public MessageSenderAdapter(ISerializerProvider serializerProvider)
        {
            _serializerProvider = serializerProvider ?? throw new NullReferenceException(nameof(serializerProvider));
        }

        public Task SendAsync(RequestMessage message)
        {
            var serializer = _serializerProvider.GetSerializer(message.Headers[Constants.ContentType]);
            if (serializer == null)
            {
                throw new NotSupportedException($"the request content-type:{message.Headers[Constants.ContentType]} is not supported!");
            }

            return SendAsync(new Message(message.Headers, serializer.SerializeObject(message.ParamterValues)));
        }

        public Task SendAsync(ResponseMessage response)
        {
            //can make the json serializer(or another) as the default serializer
            //so cannot be null at here,if the serializer is null ,
            //the response message or the error message,cannot be sent to client
            var serializer = _serializerProvider.GetSerializer(response.Headers[Constants.ContentType]);

            return SendAsync(new Message(
                response.Headers,
                serializer.SerializeObject(response.Result)
                )
            );
        }

        /// <summary>
        /// protocol to send message
        /// </summary>
        /// <param name="message"><see cref="Message"/></param>
        /// <returns><see cref="Task"/></returns>
        public abstract Task SendAsync(Message message);
    }
}
