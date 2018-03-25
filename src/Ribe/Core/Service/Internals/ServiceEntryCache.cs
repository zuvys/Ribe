using System.Collections.Concurrent;

namespace Ribe.Core.Service
{
    public class ServiceEntryCache
    {
        static ConcurrentDictionary<string, ServiceEntry> Services { get; }

        static ServiceEntryCache()
        {
            Services = new ConcurrentDictionary<string, ServiceEntry>();
        }

        public void AddOrUpdate(ServiceEntry service)
        {
            Services.AddOrUpdate(service.ServiceName, service, (k, v) => service);
        }

        public ServiceEntry Get(string serviceName)
        {
            return Services.TryGetValue(serviceName, out var service) ? service :null ;
        }

        public bool Remove(ServiceEntry service)
        {
            return Services.TryRemove(service.ServiceName, out service);
        }
    }
}
