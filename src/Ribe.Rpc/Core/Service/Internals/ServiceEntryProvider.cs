using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ribe.Rpc.Core.Service.Internals
{
    public class ServiceEntryProvider : IServiceEntryProvider
    {
        protected ServiceEntryCache Cache { get; }

        protected IServiceFactory ServiceFacotry { get; }

        public ServiceEntryProvider(IServiceFactory serviceFactory)
        {
            Cache = new ServiceEntryCache();
            ServiceFacotry = serviceFactory;
            Initialize();
        }

        protected virtual void Initialize()
        {
            foreach (var item in AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly
                    .GetExportedTypes()
                    .Where(i => i.GetCustomAttribute<ServiceAttribute>() != null)))
            {
                var entries = ServiceFacotry.CreateServices(item);
                if (entries != null)
                {
                    entries.ForEach(i =>
                    {
                        Cache.AddOrUpdate(i);
                    });
                }
            }
        }

        public ServiceEntry GetEntry(Request context)
        {
            return Cache.Get(context.ServicePath);
        }

        public IEnumerable<ServiceEntry> GetAll()
        {
            return Cache.GetAll();
        }
    }
}
