using Ribe.Core.Service.Address;
using Ribe.Messaging;

namespace Ribe.Client.Invoker.Internals
{
    public class RpcInvokerProvider : IRpcInvokerProvider
    {
        private IRpcClientFacotry _clientFacotry;

        private IMessageConvertorProvider _messageConvetorProvider;

        public RpcInvokerProvider(IRpcClientFacotry clientFacotry, IMessageConvertorProvider messageConvetorProvider)
        {
            _clientFacotry = clientFacotry;
            _messageConvetorProvider = messageConvetorProvider;
        }

        public IRpcInvoker GetInvoker()
        {
            //Select one ServiceAddress
            return new RpcInvoker(_clientFacotry, _messageConvetorProvider)
            {
                ServiceAddress = new ServiceAddress()
                {
                    Ip = "127.0.0.1",
                    Port = 8080
                }
            };
        }
    }
}
