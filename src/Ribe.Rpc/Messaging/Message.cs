using Ribe.Rpc.Core;
using System;

namespace Ribe.Rpc.Messaging
{
    public class Message
    {
        public byte[] Content { get; set; }

        public Header Header { get; set; }

        //public bool IsRequest => Header.ContainsKey(Constants.RequestKey);

        //public bool IsResponse => Header.ContainsKey(Constants.ResponseKey);

        public Message(Header header, byte[] content)
        {
            Header = header ?? throw new NullReferenceException(nameof(header));
            Content = content;
        }
    }
}