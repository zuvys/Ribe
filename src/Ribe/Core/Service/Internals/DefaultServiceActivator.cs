using System;

namespace Ribe.Core.Service.Internals
{
    public class DefaultServiceActivator : IServiceActivator
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
