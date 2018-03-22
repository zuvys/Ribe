namespace Ribe.Core.Service.Internals
{
    public class ServiceEntryProvider : IServiceEntryProvider
    {
        private ServiceEntryCache _cache;

        public ServiceEntryProvider(ServiceEntryCache cache)
        {
            _cache = cache;
        }

        public ServiceEntry GetEntry(Request context)
        {
            return _cache.Get(context.ServicePath);
        }
    }
}
