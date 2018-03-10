using Ribe.Json.Messaging;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public interface IMessageFactory
    {
        Message Create(Dictionary<string, string> headers, object content);
    }
}
