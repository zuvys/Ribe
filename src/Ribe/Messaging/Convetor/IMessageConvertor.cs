using Ribe.Core;
using System;

namespace Ribe.Messaging
{
    /// <summary>
    /// an interface can convert <see cref="Message"/> to<see cref="Response"/> and <see cref="Request"/>
    /// </summary>
    public interface IMessageConvertor
    {
        bool CanConvert(Message message);

        Response ConvertToResponse(Message message, Type valueType);

        Request ConvertToRequest(Message message);
    }
}
