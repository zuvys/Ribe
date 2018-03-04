using Ribe.Core;
using Ribe.Serialize;
using System;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public interface IMessage
    {
        byte[] Body { get; set; }

        ISerializer Serializer { get; }

        Dictionary<string, string> Headers { get; set; }

        ServiceExecutionResult GetResult(Type dataType);

        ServiceInvocationContext GetInvocationContext();
    }
}
