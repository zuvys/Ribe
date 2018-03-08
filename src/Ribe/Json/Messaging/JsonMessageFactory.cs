using Ribe.Json.Serialize;
using Ribe.Messaging;
using Ribe.Serialize;
using System.Collections.Generic;

namespace Ribe.Json.Messaging
{
    public class JsonMessageFactory : IMessageFactory
    {
        private ISerializer _serializer;

        public JsonMessageFactory(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public IMessage Create(Dictionary<string, string> headers, object content)
        {
            return new JsonMessage()
            {
                Headers = headers,
                Body = _serializer.SerializeObject(content)
            };
        }
    }
}
