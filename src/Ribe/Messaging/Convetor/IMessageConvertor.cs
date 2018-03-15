using Ribe.Core;
using System;

namespace Ribe.Messaging
{
    /// <summary>
    /// an interface can convert <see cref="Message"/> to<see cref="Result"/> and <see cref="Request"/>
    /// </summary>
    public interface IMessageConvertor
    {
        bool CanConvert(Message message);

        Result ConvertToResponse(Message message, Type valueType);

        Request ConvertToRequest(Message message);
    }
}
