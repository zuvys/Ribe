using Ribe.Codecs;
using Ribe.Json.Serialize;
using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Json.Codecs
{
    public class JsonDecoder : IDecoder
    {
        private ISerializer _serializer = new JsonSerializer();

        public Message Decode(byte[] bytes)
        {
            return _serializer.DeserializeObject<Message>(bytes);
        }
    }
}
