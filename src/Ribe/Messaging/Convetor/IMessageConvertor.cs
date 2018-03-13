using Ribe.Core;
using System;

namespace Ribe.Messaging
{
    /// <summary>
    /// an interface can convert <see cref="Message"/> to<see cref="Result"/> and <see cref="ServiceRequestContext"/>
    /// </summary>
    public interface IMessageConvertor
    {
        bool CanConvert(Message message);

        Result ConvertToResult(Message message, Type valueType);

        ServiceRequestContext ConvertToRequestContext(Message message, Type[] paramterTypes);
    }
}
