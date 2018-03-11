using Ribe.Codecs;
using Ribe.Json.Serialize;
using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Json.Codecs
{
    public class JsonDecoder : IDecoder
    {
        const string EncodingFormat = "json";

        private ISerializer _serializer = new JsonSerializer();

        public bool CanDecode(string encodingFormat)
        {
            return encodingFormat?.ToLower() == EncodingFormat;
        }

        public Message Decode(byte[] bytes)
        {
            return _serializer.DeserializeObject<Message>(bytes);
        }
    }
}
