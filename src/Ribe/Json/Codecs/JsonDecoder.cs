using Ribe.Codecs;
using Ribe.Json.Messaging;
using Ribe.Json.Serialize;
using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Json.Codecs
{
    public class JsonDecoder : IDecoder
    {
        private ISerializer _serializer = new JsonSerializer();

        public IMessage Decode(byte[] bytes)
        {
            return _serializer.DeserializeObject<JsonMessage>(bytes);
        }
    }
}
