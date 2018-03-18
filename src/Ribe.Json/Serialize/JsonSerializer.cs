using Newtonsoft.Json;
using Ribe.Serialize;

using System;
using System.Text;

namespace Ribe.Rpc.Json.Serialize
{
    public class JsonSerializer : ISerializer
    {
        public string FormatType => "json";

        public static JsonSerializer Default = new JsonSerializer();

        public byte[] SerializeObject(object value)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
        }

        public object DeserializeObject(byte[] bytes, Type type)
        {
            return DeserializeObject(Encoding.UTF8.GetString(bytes), type);
        }

        public object DeserializeObject(string value, Type type)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(value, type);
        }

        public TValue DeserializeObject<TValue>(byte[] bytes)
        {
            return (TValue)DeserializeObject(bytes, typeof(TValue));
        }

        public TValue DeserializeObject<TValue>(string value)
        {
            return (TValue)DeserializeObject(value, typeof(TValue));
        }
    }
}
