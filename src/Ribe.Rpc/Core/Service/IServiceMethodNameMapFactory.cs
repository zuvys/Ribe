using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Core.Service
{
    public interface IServiceMethodNameMapFactory
    {
        Dictionary<string, ServiceMethod> CreateMap(Type @interface, Type servieType);
    }
}
