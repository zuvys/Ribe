using Ribe.Rpc.Runtime.Client.Routing;
using System.Collections.Generic;

namespace Ribe.Rpc.Routing.Internal
{
    public class EmptyRoutingEntryProvider : IRoutingEntryProvider
    {
        public List<RoutingEntry> GetRoutes(string serivceName)
        {
            return new List<RoutingEntry>();
        }
    }
}
