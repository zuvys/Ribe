using Ribe.Rpc.Core.Service;
using Ribe.Rpc.Messaging.Formatting;
using Ribe.Rpc.Runtime.Client.Routing;
using Ribe.Rpc.Runtime.Client.Routing.Balances;
using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Invoker
{
    public class ServiceInvokerProvider : IServiceInvokerProvider
    {
        private IServiceClientFacotry _clientFacotry;

        private IMessageFormatterManager _formatterManager;

        private IRoutingEntrySelector _selector;

        private IRoutingManager _routingManager;

        private IServicePathFacotry _servicePathFacotry;

        public ServiceInvokerProvider(
            IRoutingManager routerManager,
            IServiceClientFacotry clientFacotry,
            IServicePathFacotry servicePathFacotry,
            IMessageFormatterManager formatterManager
        )
        {
            _routingManager = routerManager;
            _clientFacotry = clientFacotry;
            _formatterManager = formatterManager;
            _servicePathFacotry = servicePathFacotry;
            _selector = new RandRoutingEntrySelector();
        }

        public IServiceInvoker GetInvoker(Invocation req)
        {
            if (req.Address != null)
            {
                req.Header[Constants.ServicePath] = _servicePathFacotry.CreatePath(req.ServiceType, req.Header);

                return new ServiceInvoker(_clientFacotry.Create(req.Address), _formatterManager);
            }

            var i = 0;

            while (i++ < 5)
            {
                try
                {
                    var routes = _routingManager.Route(req);
                    if (routes != null)
                    {
                        var route = _selector.Select(routes, req);
                        if (route == null)
                        {
                            req.Header[Constants.ServicePath] = _servicePathFacotry.CreatePath(req.ServiceType, route.RouteData);

                            return new ServiceInvoker(_clientFacotry.Create(route.Address), _formatterManager);
                        }
                    }
                }
                catch
                {

                }
            }

            throw new System.Exception("创建ServiceInvoker失败!");
        }
    }
}
