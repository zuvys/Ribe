using Ribe.Messaging;

namespace Ribe.Codecs
{
    public interface IDecoder
    {
        bool CanDecode(string encodingFormat);

        Message Decode(byte[] bytes);
    }
}
