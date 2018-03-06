using Ribe.Messaging;
using Ribe.Serialize;

namespace Ribe.Client.Invoker.Internals
{
    public class DefaultServiceInvokerProvider : IServiceInvokerProvider
    {
        private ISerializer _serializer;

        private IMessageFactory _messageFactory;

        private IRpcClientFacotry _clientFacotry;

        public DefaultServiceInvokerProvider(
            ISerializer serializer,
            IMessageFactory messageFactory,
            IRpcClientFacotry clientFacotry)
        {
            _serializer = serializer;
            _messageFactory = messageFactory;
            _clientFacotry = clientFacotry;
        }

        public IServiceInvoker GetInvoker()
        {
            //Select one ServiceAddress
            return new DefaultServiceInvoker(_serializer, _messageFactory, _clientFacotry);
        }
    }
}
