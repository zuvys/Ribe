using Ribe.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public class RpcClient : IRpcClient
    {
        protected Action Close { get; }

        protected Func<IMessage, Task> SendMessage { get; }

        protected Func<string, Task<IMessage>> GetResult { get; }

        public RpcClient(
            Action close,
            Func<IMessage, Task> sendMessage,
            Func<string, Task<IMessage>> getResult)
        {
            Close = close;
            GetResult = getResult;
            SendMessage = sendMessage;
        }

        public async Task<IMessage> InvokeAsync(IMessage message)
        {
            var id = message.Headers.GetValueOrDefault(Constants.RequestId);
            if (id == null)
            {
                throw new RpcException("request id is empty!");
            }

            if (SendMessage != null)
            {
                await SendMessage(message);
            }

            return await GetResult(id);
        }

        public void Dispose()
        {
            Close?.Invoke();
        }
    }
}
