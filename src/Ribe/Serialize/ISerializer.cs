using Ribe.Messaging;
using System;

namespace Ribe.Serialize
{
    public interface ISerializer
    {
        byte[] SerializeObject(object value);

        object DeserializeObject(byte[] bytes, Type type);

        TValue DeserializeObject<TValue>(byte[] bytes);
    }
}
