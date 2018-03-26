using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing.Balances
{
    public class RandRoutingEntrySelector : IRoutingEntrySelector
    {
        public RoutingEntry Select(List<RoutingEntry> routes, Invocation req)
        {
            return routes[new Random(DateTime.Now.Millisecond).Next(0, routes.Count * 100) % routes.Count];
        }
    }
}
