using System.Collections.Generic;

namespace Ribe.Codecs
{
    public class DecoderProvider : IDecoderProvider
    {
        private IEnumerable<IDecoder> _decoders;

        public DecoderProvider(IEnumerable<IDecoder> decoders)
        {
            _decoders = decoders;
        }

        public IDecoder GetDecoder(string encodingFormat)
        {
            foreach (var decoder in _decoders)
            {
                if (decoder.CanDecode(encodingFormat))
                {
                    return decoder;
                }
            }

            return null;
        }
    }
}
