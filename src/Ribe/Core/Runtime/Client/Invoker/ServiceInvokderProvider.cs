using Ribe.Core.Service.Address;
using Ribe.Messaging;

namespace Ribe.Client.Invoker.Internals
{
    public class ServiceInvokderProvider : IServiceInvokerProvider
    {
        private IServiceClientFacotry _clientFacotry;

        private IMessageConvertorProvider _messageConvetorProvider;

        public ServiceInvokderProvider(IServiceClientFacotry clientFacotry, IMessageConvertorProvider messageConvetorProvider)
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
