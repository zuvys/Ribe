using Ribe.Core.Service.Address;

namespace Ribe.Client
{
    public interface IRpcClientFacotry
    {
        RpcClient Create(ServiceAddress address);
    }
}
