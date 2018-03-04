using System;

namespace Ribe.Core.Service
{
    public interface IServiceEntryPathFacotry
    {
        string CreatePath(Type serviceType, ServiceAttribute rpc);
    }
}
