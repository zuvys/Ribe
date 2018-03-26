using Ribe.Rpc.Core;
using System;

namespace Ribe.Rpc.Messaging.Formatting
{
    /// <summary>
    /// an interface can convert <see cref="Message"/> to<see cref="Response"/> and <see cref="Request"/>
    /// </summary>
    public interface IMessageFormatter
    {
        bool IsFormatSupported(Message message);

        Request FormatRequest(Message message);

        Response FormatResponse(Message message, Type valueType);
    }
}
