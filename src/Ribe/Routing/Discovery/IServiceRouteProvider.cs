using System.Collections.Generic;

namespace Ribe.Rpc.Routing.Discovery
{
    public interface IServiceRouteProvider
    {
        List<ServiceRoutingEntry> GetRoutes(string serivceName);
    }
}
