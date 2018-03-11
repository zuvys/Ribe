using System.Collections.Generic;

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
            foreach (var encoder in _encoders)
            {
                if (encoder.CanEncode(encodingFormat))
                {
                    return encoder;
                }
            }

            return null;
        }
    }
}
