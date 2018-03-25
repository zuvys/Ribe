using Ribe.Rpc.Core;
using System.Collections.Generic;

namespace Ribe.Rpc.Routing
{
    public class RouterManager : IRouterManager
    {
        private List<IRouter> _routers;

        public RouterManager(List<IRouter> routers)
        {
            _routers = routers ?? new List<IRouter>();
        }

        public List<ServiceRoutingEntry> Route(List<ServiceRoutingEntry> routeEntries, RequestContext req)
        {
            foreach (var router in _routers)
            {
                routeEntries = router.Route(routeEntries, req);
            }

            return routeEntries;
        }

        public void AddRouter(IRouter router)
        {
            if (!_routers.Contains(router))
            {
                _routers.Add(router);
            }
        }

        public void RemoveRouter(IRouter router)
        {
            if (_routers.Contains(router))
            {
                _routers.Remove(router);
            }
        }
    }
}
