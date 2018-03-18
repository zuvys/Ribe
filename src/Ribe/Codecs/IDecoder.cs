using Ribe.Messaging;

namespace Ribe.Codecs
{
    public interface IDecoder
    {
        bool CanDecode(string formatType);

        Message Decode(byte[] bytes);
    }
}
