using Ribe.Rpc.Core;
using System.Collections.Generic;
using System.Linq;

namespace Ribe.Rpc.Routing.Routers
{
    /// <summary>
    /// method name prefix
    /// </summary>
    public class ServiceMethodRouter : IRouter
    {
        public virtual List<RoutingEntry> Route(List<RoutingEntry> routes, RequestContext req)
        {
            var routedEntries = new List<RoutingEntry>();

            foreach (var route in routes)
            {
                var names = route.Descriptions.GetValueOrDefault(Constants.MethodName, string.Empty);
                if (string.IsNullOrEmpty(names))
                {
                    routedEntries.Add(route);
                    continue;
                }

                if (names.Split(";").Any(i => req.Header.GetValueOrDefault(Constants.MethodName).StartsWith(i)))
                {
                    routedEntries.Add(route);
                }
            }

            return routedEntries;
        }
    }
}
