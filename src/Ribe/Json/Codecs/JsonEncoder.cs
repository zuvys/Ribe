using Ribe.Codecs;
using Ribe.Messaging;

namespace Ribe.Json.Codecs
{
    public class JsonEncoder : IEncoder
    {
        public byte[] Encode(Message message)
        {
            return message.Serializer.SerializeObject(message);
        }
    }
}
