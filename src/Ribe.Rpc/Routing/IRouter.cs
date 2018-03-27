using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing
{
    public interface IRouter
    {
        List<RoutingEntry> Route(List<RoutingEntry> routes, Invocation req);
    }
}
