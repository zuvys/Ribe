using System;

namespace Ribe.Rpc.Core.Service.Internals
{
    public class ServiceActivator : IServiceActivator
    {
        public object Create(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }

        public void Release(object instance)
        {
            if (instance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
