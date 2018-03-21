using Ribe.Core.Service.Address;

namespace Ribe.Rpc.Server
{
    public interface IRpcServerFactory
    {
        RpcServer Create(ServiceAddress address);
    }
}
