using Ribe.Core.Service.Address;
using Ribe.Infrustructure;
using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Client.Invoker.Internals
{
    public class DefaultServiceInvokerProvider : IServiceInvokerProvider
    {
        private IMessageFactory _messageFactory;

        private IRpcClientFacotry _clientFacotry;

        private IIdGenerator _idGenerator;

        public DefaultServiceInvokerProvider(
            IIdGenerator idGenerator,
            IMessageFactory messageFactory,
            IRpcClientFacotry clientFacotry)
        {
            _idGenerator = idGenerator;
            _messageFactory = messageFactory;
            _clientFacotry = clientFacotry;
        }

        public IServiceInvoker GetInvoker()
        {
            //Select one ServiceAddress
            return new DefaultServiceInvoker( _idGenerator, _messageFactory, _clientFacotry)
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
