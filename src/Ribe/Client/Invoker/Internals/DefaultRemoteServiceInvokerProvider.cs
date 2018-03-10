using Ribe.Core.Service.Address;
using Ribe.Infrustructure;
using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Client.Invoker.Internals
{
    public class DefaultRemoteServiceInvokerProvider : IRemoteServiceInvokerProvider
    {
        private IMessageFactory _messageFactory;

        private IClientFacotry _clientFacotry;

        private IIdGenerator _idGenerator;

        public DefaultRemoteServiceInvokerProvider(
            IIdGenerator idGenerator,
            IMessageFactory messageFactory,
            IClientFacotry clientFacotry)
        {
            _idGenerator = idGenerator;
            _messageFactory = messageFactory;
            _clientFacotry = clientFacotry;
        }

        public IRemoteServiceInvoker GetInvoker()
        {
            //Select one ServiceAddress
            return new DefaultRemoteServiceInvoker( _idGenerator, _messageFactory, _clientFacotry)
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
