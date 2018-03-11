using Ribe.Core.Service.Address;

namespace Ribe.Client
{
    public interface IRpcClientFacotry
    {
        IRpcClient CreateClient(ServiceAddress address);
    }
}
