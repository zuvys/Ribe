using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ribe.Rpc.Codecs
{
    public class EncoderManager : IEncoderManager
    {
        private ConcurrentDictionary<Type, IEncoder> _encoders;

        public EncoderManager()
        {
            _encoders = new ConcurrentDictionary<Type, IEncoder>();
        }

        public IEncoder GetEncoder(string encodingFormat)
        {
            return _encoders.Values.FirstOrDefault(i => i.CanEncode(encodingFormat));
        }

        public void AddEncoder(IEncoder encoder)
        {
            _encoders.AddOrUpdate(encoder.GetType(), encoder, (k, v) => encoder);
        }

        public void RemoveEncoder(IEncoder encoder)
        {
            _encoders.TryRemove(encoder.GetType(), out var _);
        }
    }
}
