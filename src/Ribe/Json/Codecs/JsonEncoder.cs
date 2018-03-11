using Ribe.Codecs;
using Ribe.Json.Serialize;
using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Json.Codecs
{
    public class JsonEncoder : IEncoder
    {
        const string EncodingFormat = "json";

        private ISerializer _serializer = new JsonSerializer();

        public byte[] Encode(Message message)
        {
            return _serializer.SerializeObject(message);
        }

        public bool CanEncode(string encodingFormat)
        {
            return encodingFormat?.ToLower() == EncodingFormat;
        }
    }
}
