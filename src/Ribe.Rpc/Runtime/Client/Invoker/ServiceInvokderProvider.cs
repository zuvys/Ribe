using Ribe.Rpc.Core.Service;
using Ribe.Rpc.Messaging.Formatting;
using Ribe.Rpc.Runtime.Client.Routing;
using Ribe.Rpc.Runtime.Client.Routing.Balances;

namespace Ribe.Rpc.Runtime.Client.Invoker
{
    public class ServiceInvokderProvider : IServiceInvokerProvider
    {
        private IServiceClientFacotry _clientFacotry;

        private IMessageFormatterManager _formatterManager;

        private IRoutingEntrySelector _selector;

        private IRoutingEntryProvider _routeProvider;

        private IRoutingManager _routerManager;

        private IServicePathFacotry _servicePathFacotry;

        public ServiceInvokderProvider(
            IRoutingManager routerManager,
            IRoutingEntryProvider routeProvider,
            IServiceClientFacotry clientFacotry,
            IServicePathFacotry servicePathFacotry,
            IMessageFormatterManager formatterManager
        )
        {
            _routerManager = routerManager;
            _routeProvider = routeProvider;
            _clientFacotry = clientFacotry;
            _formatterManager = formatterManager;
            _servicePathFacotry = servicePathFacotry;
            _selector = new RandRoutingEntrySelector();
        }

        public IServiceInvoker GetInvoker(Invocation req)
        {
            var routes = _routeProvider.GetRoutes(req.Header[Constants.ServiceName]);
            var routedRoutes = _routerManager.Route(routes, req);

            if (routedRoutes == null || routedRoutes.Count == 0)
            {
                //log
            }

            //AddRouteData To Generate Url
            var selectedRoute = _selector.Select(routedRoutes, req);
            if (selectedRoute != null)
            {
                req.Header[Constants.ServicePath] = _servicePathFacotry.CreatePath(req.ServiceType, selectedRoute.Descriptions);
            }

            System.Console.WriteLine(selectedRoute.Address);

            return new ServiceInvoker(_clientFacotry.Create(selectedRoute.Address), _formatterManager);
        }
    }
}
