using System;
using System.Collections.Generic;

namespace Ribe.Core.Service
{
    public interface IServiceNameFacotry
    {
        string CreateName(Type serviceType, ServiceAttribute rpc);

        string CreateName(Type serviceType, Dictionary<string, string> description);
    }
}
