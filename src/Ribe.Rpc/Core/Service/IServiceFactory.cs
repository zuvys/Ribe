using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Core.Service
{
    public interface IServiceFactory
    {
        List<ServiceEntry> CreateServices(Type serviceType);
    }
}
