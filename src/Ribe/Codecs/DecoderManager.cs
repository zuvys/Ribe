using System.Linq;
using System.Collections.Concurrent;
using System;

namespace Ribe.Codecs
{
    public class DecoderManager : IDecoderManager
    {
        private ConcurrentDictionary<Type, IDecoder> _decoders;

        public DecoderManager()
        {
            _decoders = new ConcurrentDictionary<Type, IDecoder>();
        }

        public IDecoder GetDecoder(string encodingFormat)
        {
            return _decoders.Values.FirstOrDefault(i => i.CanDecode(encodingFormat));
        }

        public void RemoveDecoder(IDecoder decoder)
        {
            _decoders.TryRemove(decoder.GetType(), out var _);
        }

        public void AddDecoder(IDecoder decoder)
        {
            _decoders.AddOrUpdate(decoder.GetType(), decoder, (k, v) => decoder);
        }
    }
}
