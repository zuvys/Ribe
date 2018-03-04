using System;

namespace Ribe.Core.Service
{
    public interface IServiceActivator
    {
        object Create(Type serviceType);

        void Release(object instance);
    }
}
