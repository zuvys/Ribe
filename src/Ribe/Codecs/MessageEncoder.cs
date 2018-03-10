using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Codecs
{
    public class MessageEncoder : IEncoder
    {
        private ISerializerProvider _serializerProvider;

        public MessageEncoder(ISerializerProvider serializerProvider)
        {
            _serializerProvider = serializerProvider;
        }

        public byte[] Encode(Message message)
        {
            return _serializerProvider.GetSerializer().SerializeObject(message);
        }
    }
}
