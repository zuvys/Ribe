using Ribe.Messaging;
using Ribe.Serialize;
using System.Collections.Generic;

namespace Ribe.Json.Messaging
{
    //removed
    public class JsonMessageFactory : IMessageFactory
    {
        private ISerializer _serializer;

        public JsonMessageFactory(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public Message Create(Dictionary<string, string> headers, object content)
        {
            return new Message()
            {
                Headers = headers,
                Content = _serializer.SerializeObject(content)
            };
        }
    }
}
