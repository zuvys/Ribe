using System;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public class Message 
    {
        public byte[] Content { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Message(Dictionary<string, string> headers, byte[] content)
        {
            Headers = headers ?? throw new NullReferenceException(nameof(headers));
            Content = content;
        }
    }
}