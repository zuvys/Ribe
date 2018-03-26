using System.Collections.Generic;

namespace Ribe.Rpc.Runtime.Client.Routing.Routers
{
    public class ServiceGroupRouter : IRouter
    {
        public List<RoutingEntry> Route(List<RoutingEntry> entries, Invocation req)
        {
            if (entries == null || entries.Count == 0)
            {
                return entries;
            }

            var routedEntries = new List<RoutingEntry>();

            foreach (var entry in entries)
            {
                if (entry.Descriptions.GetValueOrDefault(Constants.Group) == req.Header.GetValueOrDefault(Constants.Group))
                {
                    routedEntries.Add(entry);
                }
            }

            return routedEntries;
        }
    }
}
