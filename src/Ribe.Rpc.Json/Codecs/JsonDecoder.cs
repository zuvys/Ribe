using Ribe.Rpc.Codecs;
using Ribe.Rpc.Json.Serialize;
using Ribe.Rpc.Messaging;
using System;

namespace Ribe.Rpc.Json.Codecs
{
    public class JsonDecoder : IDecoder
    {
        const string FormatType = "json";

        public Message Decode(byte[] bytes)
        {
            return JsonSerializer.Default.DeserializeObject<Message>(bytes);
        }

        public bool CanDecode(string formatType)
        {
            return string.Equals(formatType, FormatType, StringComparison.OrdinalIgnoreCase);
        }
    }
}
