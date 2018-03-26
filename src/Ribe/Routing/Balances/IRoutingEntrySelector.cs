using Ribe.Rpc.Core;
using System.Collections.Generic;

namespace Ribe.Rpc.Routing.Balances
{
    public interface IRoutingEntrySelector
    {
        RoutingEntry Select(List<RoutingEntry> routes, RequestContext req);
    }
}
