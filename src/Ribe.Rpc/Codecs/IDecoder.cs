using Ribe.Rpc.Messaging;

namespace Ribe.Rpc.Codecs
{
    public interface IDecoder
    {
        bool CanDecode(string formatType);

        Message Decode(byte[] bytes);
    }
}
