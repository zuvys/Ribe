using System.Collections.Generic;
using System.Linq;

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
            return _decoders.FirstOrDefault(i => i.CanDecode(encodingFormat));
        }
    }
}
