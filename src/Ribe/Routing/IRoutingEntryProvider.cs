using System.Collections.Generic;

namespace Ribe.Rpc.Routing.Discovery
{
    public interface IRoutingEntryProvider
    {
        List<RoutingEntry> GetRoutes(string serivceName);
    }
}
