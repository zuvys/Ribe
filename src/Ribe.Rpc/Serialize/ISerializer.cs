using System;

namespace Ribe.Rpc.Serialize
{
    public interface ISerializer
    {
        string FormatType { get; }

        byte[] SerializeObject(object value);

        object DeserializeObject(byte[] bytes, Type type);

        TValue DeserializeObject<TValue>(byte[] bytes);
    }
}
