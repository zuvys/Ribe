using Ribe.Rpc.Core;
using System.Collections.Generic;

namespace Ribe.Rpc.Routing
{
    public interface IRouter
    {
        List<ServiceRoutingEntry> Route(List<ServiceRoutingEntry> routes, RequestContext req);
    }
}
