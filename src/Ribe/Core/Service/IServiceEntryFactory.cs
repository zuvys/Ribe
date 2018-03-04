using System;
using System.Collections.Generic;

namespace Ribe.Core.Service
{
    public interface IServiceEntryFactory
    {
        List<ServiceEntry> CreateServices(Type serviceType);
    }
}
