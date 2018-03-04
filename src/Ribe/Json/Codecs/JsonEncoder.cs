using Ribe.Codecs;
using Ribe.Messaging;

namespace Ribe.Json.Codecs
{
    public class JsonEncoder : IEncoder
    {
        public byte[] Encode(IMessage mesage)
        {
            return mesage.Serializer.SerializeObject(mesage);
        }
    }
}
