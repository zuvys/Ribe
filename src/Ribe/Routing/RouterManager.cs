using Ribe.Rpc.Core;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using Ribe.Rpc.Routing.Routers;

namespace Ribe.Rpc.Routing
{
    public class RouterManager : IRouterManager
    {
        private ConcurrentDictionary<Type, IRouter> _routers;

        public RouterManager()
        {
            _routers = new ConcurrentDictionary<Type, IRouter>();

            AddRouter(new ServiceMethodRouter());
        }

        public List<ServiceRoutingEntry> Route(List<ServiceRoutingEntry> routeEntries, RequestContext req)
        {
            foreach (var router in _routers.Values)
            {
                routeEntries = router.Route(routeEntries, req);
            }

            return routeEntries;
        }

        public void AddRouter(IRouter router)
        {
            _routers.AddOrUpdate(router.GetType(), router, (k, v) => router);
        }

        public void RemoveRouter(IRouter router)
        {
            _routers.TryRemove(router.GetType(), out var _);
        }
    }
}
