using Ribe.Rpc.Runtime.Client.Routing.Routers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing
{
    public class RoutingManager : IRoutingManager
    {
        private IRoutingEntryProvider _routingEntryProvider { get; }

        protected ConcurrentDictionary<Type, IRouter> Routers = new ConcurrentDictionary<Type, IRouter>();

        public RoutingManager(IRoutingEntryProvider routingEntryProvider)
        {
            AddRouter(new ServiceMethodRouter());
            AddRouter(new ServiceVersionRouter());
            AddRouter(new ServiceGroupRouter());

            _routingEntryProvider = routingEntryProvider;
        }

        public void AddRouter(IRouter router)
        {
            Routers.AddOrUpdate(router.GetType(), router, (k, v) => router);
        }

        public void RemoveRouter(IRouter router)
        {
            Routers.TryRemove(router.GetType(), out var _);
        }

        public List<RoutingEntry> Route(Invocation invocation)
        {
            var routes = _routingEntryProvider.GetRoutes(invocation.Header.GetValueOrDefault(Constants.ServiceName));
            if (routes == null || routes.Count == 0)
            {
                return routes;
            }

            foreach (var router in Routers.Values)
            {
                routes = router.Route(routes, invocation);
            }

            return routes;
        }
    }
}
