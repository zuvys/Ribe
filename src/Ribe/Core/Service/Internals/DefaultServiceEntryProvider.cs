﻿namespace Ribe.Core.Service.Internals
{
    public class DefaultServiceEntryProvider : IServiceEntryProvider
    {
        private ServiceEntryCache _cache;

        public DefaultServiceEntryProvider(ServiceEntryCache cache)
        {
            _cache = cache;
        }

        public ServiceEntry GetServiceEntry(ServiceInvocationContext context)
        {
            return _cache.Get(context.ServicePath);
        }
    }
}