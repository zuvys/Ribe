using Ribe.Rpc.Core;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Runtime.Client
{
    public class Invocation
    {
        public bool IsVoid { get; }

        public bool IsAsync { get; }

        public Header Header { get; }

        public Type ValueType { get; }

        public Type ServiceType { get; }

        public object[] ParamterValues { get; }

        public Invocation(Header header, Type serviceType, object[] paramterValues, Type responseValueType)
        {
            IsVoid = responseValueType == null || typeof(Task) == responseValueType || typeof(void) == responseValueType;
            IsAsync = responseValueType != null && typeof(Task).IsAssignableFrom(responseValueType);

            Header = header;
            ServiceType = serviceType;

            ParamterValues = paramterValues;

            ValueType = IsAsync && !IsVoid
                ? responseValueType.GetGenericArguments()[0]
                : responseValueType;
        }
    }
}
