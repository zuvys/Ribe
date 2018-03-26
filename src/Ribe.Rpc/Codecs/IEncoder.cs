using Ribe.Rpc.Messaging;

namespace Ribe.Rpc.Codecs
{
    public interface IEncoder
    {
        bool CanEncode(string formatType);

        byte[] Encode(Message message);
    }
}
