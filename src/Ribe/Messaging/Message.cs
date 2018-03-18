using System;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public class Message
    {
        public byte[] Content { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public bool IsRequest => Headers.ContainsKey(Constants.RequestKey);

        public bool IsResponse => Headers.ContainsKey(Constants.ResponseKey);

        public Message(Dictionary<string, string> headers, byte[] content)
        {
            Headers = headers ?? throw new NullReferenceException(nameof(headers));
            Content = content;
        }
    }
}