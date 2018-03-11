namespace Ribe.Codecs
{
    public interface IDecoderProvider
    {
        IDecoder GetDecoder(string contentType);
    }
}
