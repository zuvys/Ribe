using Ribe.Core;
using Ribe.Core.Service.Address;
using Ribe.Messaging;
using Ribe.Rpc.Core.Runtime.Client.Selector;
using Ribe.Rpc.Routing;
using Ribe.Rpc.Routing.Discovery;
using Ribe.Rpc.Routing.Routers;
using System.Collections.Generic;
using System.Linq;

namespace Ribe.Rpc.Core.Runtime.Client.Invoker
{
    public class ServiceInvokderProvider : IServiceInvokerProvider
    {
        private IServiceClientFacotry _clientFacotry;

        private IMessageFormatterProvider _formatterProvider;

        private IServiceAddressSelector _selector;

        private IServiceRouteProvider _routeProvider;

        public ServiceInvokderProvider(
            IServiceRouteProvider routeProvider,
            IServiceClientFacotry clientFacotry,
            IMessageFormatterProvider formatterProvider
        )
        {
            _routeProvider = routeProvider;
            _clientFacotry = clientFacotry;
            _formatterProvider = formatterProvider;
            _selector = new RandomServiceAddressSelector();
        }

        public IServiceInvoker GetInvoker(RequestContext req)
        {
            var routes = new RouterManager(new List<IRouter> { new ServiceMethodRouter() })
                  .Route(_routeProvider.GetRoutes(req.Header[Constants.ServiceName]), req);

            var host = _selector.Select(routes.Select(i => i.Address).ToList(), req);

            return new ServiceInvoker(_clientFacotry, _formatterProvider)
            {
                Address = host
            };
        }
    }
}
