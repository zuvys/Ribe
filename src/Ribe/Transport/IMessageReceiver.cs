using System;
using Ribe.Messaging;
using System.Threading.Tasks;

namespace Ribe.Rpc.Transport
{
    public interface IMessageReceiver
    {
        /// <summary>
        /// Receive Message,if the message is a response,the value type is void
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReceiveAsync(Message message);

        /// <summary>
        /// Receive Message,
        /// if the message is a response,the value type is valueType
        /// if the message is request,the valuetype is useless
        /// </summary>
        /// <param name="message"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        Task ReceiveAsync(Message message, Type valueType);
    }
}
