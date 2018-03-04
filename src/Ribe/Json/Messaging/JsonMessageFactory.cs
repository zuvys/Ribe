using Ribe.Messaging;
using System.Collections.Generic;

namespace Ribe.Json.Messaging
{
    public class JsonMessageFactory : IMessageFactory
    {
        public IMessage Create(Dictionary<string, string> headers, byte[] body)
        {
            return new JsonMessage()
            {
                Headers = headers,
                Body = body
            };
        }
    }
}
