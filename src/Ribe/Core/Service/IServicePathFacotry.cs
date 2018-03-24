using System;

namespace Ribe.Core.Service
{
    public interface IServicePathFacotry
    {
        string CreatePath(Type serviceType, ServiceAttribute rpc);
    }
}
