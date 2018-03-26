namespace Ribe.Rpc.Codecs
{
    public interface IEncoderManager
    {
        IEncoder GetEncoder(string contentType);

        void AddEncoder(IEncoder encoder);

        void RemoveEncoder(IEncoder encoder);
    }
}
