using Ribe.Core.Service.Address;
using Ribe.Messaging;

namespace Ribe.Rpc.Core.Runtime.Client.Invoker
{
    public class ServiceInvokderProvider : IServiceInvokerProvider
    {
        private IServiceClientFacotry _clientFacotry;

        private IMessageFormatterProvider _messageConvetorProvider;

        public ServiceInvokderProvider(IServiceClientFacotry clientFacotry, IMessageFormatterProvider messageConvetorProvider)
        {
            _clientFacotry = clientFacotry;
            _messageConvetorProvider = messageConvetorProvider;
        }

        public IServiceInvoker GetInvoker()
        {
            //Select one ServiceAddress
            return new ServiceInvoker(_clientFacotry, _messageConvetorProvider)
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
