using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ribe.Rpc.Serialize
{
    public class SerializerManager : ISerializerManager
    {
        private ConcurrentDictionary<string, ISerializer> _serializers;

        public SerializerManager()
        {
            _serializers = new ConcurrentDictionary<string, ISerializer>();
        }

        public ISerializer GetSerializer(string formatType)
        {
            return _serializers.FirstOrDefault(i => string.Equals(i.Key, formatType, StringComparison.OrdinalIgnoreCase)).Value;
        }

        public void AddSerializer(ISerializer serializer)
        {
            _serializers.AddOrUpdate(serializer.FormatType, serializer, (k, v) => serializer);
        }

        public void RemoveSerializer(ISerializer serializer)
        {
            _serializers.TryRemove(serializer.FormatType, out var _);
        }
    }
}
