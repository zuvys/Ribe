using Ribe.Core;
using Ribe.Serialize;
using System;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public interface IMessage
    {
        byte[] Body { get; set; }

        Dictionary<string, string> Headers { get; set; }

        ISerializer Serializer { get; }

        Result GetResult(Type dataType);

        InvokeContext GetInvokeContext();
    }
}
