using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing.Balances
{
    public interface IRoutingEntrySelector
    {
        RoutingEntry Select(List<RoutingEntry> routes, Invocation req);
    }
}
