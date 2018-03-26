using Ribe.Rpc.Runtime.Client.Routing.Routers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing
{
    public class RoutingManager : IRoutingManager
    {
        private ConcurrentDictionary<Type, IRouter> _routers;

        public RoutingManager()
        {
            _routers = new ConcurrentDictionary<Type, IRouter>();

            AddRouter(new ServiceMethodRouter());
            AddRouter(new ServiceVersionRouter());
            AddRouter(new ServiceGroupRouter());
        }

        public List<RoutingEntry> Route(List<RoutingEntry> entries, Invocation req)
        {
            foreach (var router in _routers.Values)
            {
                entries = router.Route(entries, req);
            }

            return entries;
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
