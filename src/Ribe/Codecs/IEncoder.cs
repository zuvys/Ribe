using Ribe.Messaging;

namespace Ribe.Codecs
{
    public interface IEncoder
    {
        bool CanEncode(string encodingFormat);

        byte[] Encode(Message message);
    }
}
