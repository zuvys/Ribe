using System.Collections.Generic;

namespace Ribe.Messaging
{
    public interface IMessageFactory
    {
        IMessage Create(Dictionary<string, string> headers, object content);
    }
}
