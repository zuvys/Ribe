using System;

namespace Ribe.Rpc.Core.Service
{
    public interface IServiceActivator
    {
        object Create(Type serviceType);

        void Release(object instance);
    }
}
