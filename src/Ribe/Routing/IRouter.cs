using Ribe.Rpc.Core;
using System.Collections.Generic;

namespace Ribe.Rpc.Routing
{
    public interface IRouter
    {
        List<RoutingEntry> Route(List<RoutingEntry> routes, RequestContext req);
    }
}
