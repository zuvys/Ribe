using System;
using System.Linq;
using System.Reflection;

namespace Ribe.Core.Service.Internals
{
    public class ServiceProvider : IServiceProvider
    {
        protected ServiceEntryCache Cache { get; set; }

        protected IServiceFactory ServiceFacotry { get; set; }

        public ServiceProvider(IServiceFactory serviceFactory)
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
                    entries.ForEach(i => Cache.AddOrUpdate(i));
                }
            }
        }

        public ServiceEntry GetEntry(Request context)
        {
            return Cache.Get(context.ServicePath);
        }
    }
}
