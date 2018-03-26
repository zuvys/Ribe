using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Core.Service
{
    public interface IServicePathFacotry
    {
        string CreatePath(Type serviceType, ServiceAttribute rpc);

        string CreatePath(Type serviceType, Dictionary<string, string> description);

        string CreatePath(string serviceName, Dictionary<string, string> description);
    }
}
