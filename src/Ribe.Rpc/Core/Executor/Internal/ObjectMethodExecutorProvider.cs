using Ribe.Rpc.Core.Service;
using System;
using System.Collections.Concurrent;

namespace Ribe.Rpc.Core.Executor.Internals
{
    public class ObjectMethodExecutorProvider : IObjectMethodExecutorProvider
    {
        private ConcurrentDictionary<ObjectMethodExecutorCacheKey, IObjectMethodExecutor> _methodExecutors;

        public ObjectMethodExecutorProvider()
        {
            _methodExecutors = new ConcurrentDictionary<ObjectMethodExecutorCacheKey, IObjectMethodExecutor>();
        }

        public IObjectMethodExecutor GetExecutor(ExecutionContext context)
        {
            var cacheKey = new ObjectMethodExecutorCacheKey()
            {
                ServiceType = context.ServiceType,
                ServiceMethod = context.ServiceMethod
            };

            return _methodExecutors.GetOrAdd(cacheKey, new ObjectMethodExecutor(context.ServiceType, context.ServiceMethod));
        }
    }

    internal class ObjectMethodExecutorCacheKey
    {
        public Type ServiceType { get; set; }

        public ServiceMethod ServiceMethod { get; set; }

        public override int GetHashCode()
        {
            return (ServiceMethod.GetHashCode() * 31) ^ ServiceType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            var o = (ObjectMethodExecutorCacheKey)obj;
            if (o == null)
            {
                return false;
            }

            return o.ServiceMethod == ServiceMethod && o.ServiceType == ServiceType;
        }
    }
}
