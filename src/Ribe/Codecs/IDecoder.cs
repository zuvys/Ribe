using Ribe.Messaging;

namespace Ribe.Codecs
{
    public interface IDecoder
    {
        IMessage Decode(byte[] bytes);
    }
}
