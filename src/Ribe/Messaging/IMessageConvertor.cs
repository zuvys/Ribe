using Ribe.Core;
using Ribe.Json.Messaging;
using System;

namespace Ribe.Messaging
{
    /// <summary>
    /// an interface can convert <see cref="Message"/> to<see cref="Result"/> and <see cref="ServiceContext"/>
    /// </summary>
    public interface IMessageConvertor
    {
        bool CanConvert(Message message);

        Result ConvertToResult(Message message, Type valueType);

        ServiceContext ConvertToServiceContext(Message message, Type[] paramterTypes);
    }
}
