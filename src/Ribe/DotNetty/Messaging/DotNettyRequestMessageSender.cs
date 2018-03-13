using DotNetty.Transport.Channels;
using Ribe.Messaging;
using Ribe.Serialize;

using System;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Messaging
{
    public class DotNettyRequestMessageSender : DotNettyMessageSender, IRequestMessageSender, IDisposable
    {
        private ISerializerProvider _serializerProvider;

        public DotNettyRequestMessageSender(IChannel channel, ISerializerProvider serializerProvider)
            : base(channel)
        {
            _serializerProvider = serializerProvider ?? throw new NullReferenceException(nameof(serializerProvider));
        }

        public Task SendAsync(RequestMessage message)
        {
            var serializer = _serializerProvider.GetSerializer(message.Headers[Constants.ContentType]);
            if (serializer == null)
            {
                throw new NotFiniteNumberException(nameof(serializer));
            }

            return SendAsync(new Message(message.Headers, serializer.SerializeObject(message.ParamterValues)));
        }

        public void Dispose()
        {
            Channel.DisconnectAsync().Wait();
        }
    }
}
