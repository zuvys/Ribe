using System.Collections.Generic;
using System.Linq;

namespace Ribe.Serialize.Internal
{
    public class SerializerProvider : ISerializerProvider
    {
        private IEnumerable<ISerializer> _serializers;

        public SerializerProvider(IEnumerable<ISerializer> serializers)
        {
            _serializers = serializers ?? new List<ISerializer>();
        }

        public ISerializer GetSerializer(string formatType)
        {
            return _serializers.FirstOrDefault(i => i.FormatType == formatType);
        }
    }
}
