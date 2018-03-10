using Ribe.Messaging;

namespace Ribe.Codecs
{
    public interface IDecoder
    {
        Message Decode(byte[] bytes);
    }
}
