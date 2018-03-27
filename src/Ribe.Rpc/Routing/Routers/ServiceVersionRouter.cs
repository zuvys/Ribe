using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing.Routers
{
    public class ServiceVersionRouter : IRouter
    {
        public List<RoutingEntry> Route(List<RoutingEntry> entries, Invocation req)
        {
            if (entries == null || entries.Count == 0)
            {
                return entries;
            }

            if (!req.Header.ContainsKey(Constants.Version))
            {
                return entries;
            }

            var routedEntries = new List<RoutingEntry>();

            foreach (var entry in entries)
            {
                if (entry.RouteData.GetValueOrDefault(Constants.Version) == req.Header.GetValueOrDefault(Constants.Version))
                {
                    routedEntries.Add(entry);
                }
            }

            return routedEntries;
        }
    }
}
