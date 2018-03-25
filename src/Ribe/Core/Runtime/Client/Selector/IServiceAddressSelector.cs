using Ribe.Core.Service.Address;
using System.Collections.Generic;

namespace Ribe.Rpc.Core.Runtime.Client
{
    public interface IServiceAddressSelector
    {
        ServiceAddress Select(List<ServiceAddress> addresses, RequestContext req);
    }
}
