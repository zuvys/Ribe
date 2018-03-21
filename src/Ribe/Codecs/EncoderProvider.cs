using System.Collections.Generic;
using System.Linq;

namespace Ribe.Codecs
{
    public class EncoderProvider : IEncoderProvider
    {
        private IEnumerable<IEncoder> _encoders;

        public EncoderProvider(IEnumerable<IEncoder> encoders)
        {
            _encoders = encoders;
        }

        public IEncoder GetEncoder(string encodingFormat)
        {
            return _encoders.FirstOrDefault(i => i.CanEncode(encodingFormat));
        }
    }
}
