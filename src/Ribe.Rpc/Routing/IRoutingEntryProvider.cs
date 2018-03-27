using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing
{
    public interface IRoutingEntryProvider
    {
        List<RoutingEntry> GetRoutes(string serivceName);
    }
}
