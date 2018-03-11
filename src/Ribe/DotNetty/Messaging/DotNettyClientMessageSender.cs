using DotNetty.Transport.Channels;
using Ribe.Messaging;
using Ribe.Serialize;

using System;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Messaging
{
    public class DotNettyClientMessageSender : DotNettyMessageSender, IClientMessageSender, IDisposable
    {
        private ISerializerProvider _serializerProvider;

        public DotNettyClientMessageSender(IChannel channel, ISerializerProvider serializerProvider)
            : base(channel)
        {
            _serializerProvider = serializerProvider ?? throw new NullReferenceException(nameof(serializerProvider));
        }

        public Task SendAsync(RemoteCallMessage invokeMessage)
        {
            var serializer = _serializerProvider.GetSerializer(invokeMessage.Headers[Constants.ContentType]);
            if (serializer == null)
            {
                throw new NotFiniteNumberException(nameof(serializer));
            }

            var message = new Message(
                invokeMessage.Headers,
                serializer.SerializeObject(invokeMessage.ParamterValues));

            return SendAsync(message);
        }

        public void Dispose()
        {
            Channel.DisconnectAsync().Wait();
        }
    }
}
