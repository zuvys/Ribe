using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing
{
    public interface IRoutingManager
    {
        void AddRouter(IRouter router);

        void RemoveRouter(IRouter router);

        List<RoutingEntry> Route(Invocation invocation);
    }
}
