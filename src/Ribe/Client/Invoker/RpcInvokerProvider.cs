using Ribe.Core.Service.Address;

namespace Ribe.Client.Invoker.Internals
{
    public class RpcInvokerProvider : IRpcInvokerProvider
    {
        private IRpcClientFacotry _clientFacotry;

        public RpcInvokerProvider(IRpcClientFacotry clientFacotry)
        {
            _clientFacotry = clientFacotry;
        }

        public IRpcInvoker GetInvoker()
        {
            //Select one ServiceAddress
            return new RpcInvoker(_clientFacotry, null)
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
