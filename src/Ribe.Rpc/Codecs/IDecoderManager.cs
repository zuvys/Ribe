namespace Ribe.Rpc.Codecs
{
    public interface IDecoderManager
    {
        IDecoder GetDecoder(string contentType);

        void AddDecoder(IDecoder decoder);

        void RemoveDecoder(IDecoder decoder);
    }
}
