namespace Ribe.Codecs
{
    public interface IEncoderProvider
    {
        IEncoder GetEncoder(string contentType);
    }
}
