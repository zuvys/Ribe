using Ribe.Core.Service.Address;
using Ribe.Rpc.Core;
using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Routing.Balances
{
    public class RandRoutingEntrySelector : IRoutingEntrySelector
    {
        public RoutingEntry Select(List<RoutingEntry> routes, RequestContext req)
        {
            return routes[new Random(DateTime.Now.Millisecond).Next(0, routes.Count * 100) % routes.Count];
        }
    }
}
