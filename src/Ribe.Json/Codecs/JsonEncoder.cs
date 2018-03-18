using Ribe.Codecs;
using Ribe.Messaging;
using Ribe.Rpc.Json.Serialize;
using System;

namespace Ribe.Rpc.Json.Codecs
{
    public class JsonEncoder : IEncoder
    {
        const string FormatType = "json";

        public byte[] Encode(Message message)
        {
            return JsonSerializer.Default.SerializeObject(message);
        }

        public bool CanEncode(string formatType)
        {
            return string.Equals(FormatType, formatType, StringComparison.OrdinalIgnoreCase);
        }
    }
}
