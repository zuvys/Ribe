using System;
using System.Collections.Generic;

namespace Ribe.Core.Service
{
    public interface IServiceMethodMapFacotry
    {
        Dictionary<string, ServiceMethod> CreateMethodMap(Type @interface, Type servieType);
    }
}
