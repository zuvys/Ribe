using Ribe.Messaging;

namespace Ribe.Codecs
{
    public interface IEncoder
    {
        byte[] Encode(Message message);
    }
}
