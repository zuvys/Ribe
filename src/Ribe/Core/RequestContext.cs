using Ribe.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core
{
    public class RequestContext
    {
        public Header Header { get; }

        public Type ServiceType { get; }

        public Type ResponseValueType { get; }

        public object[] RequestParamterValues { get; }

        public bool IsVoidRequest { get; }

        public bool IsAsyncRequest { get; }

        public RequestContext(Header header, Type serviceType, object[] paramterValues, Type responseValueType)
        {
            IsVoidRequest = responseValueType == null || typeof(Task) == responseValueType || typeof(void) == responseValueType;
            IsAsyncRequest = responseValueType != null && typeof(Task).IsAssignableFrom(responseValueType);

            Header = header;
            ServiceType = serviceType;

            RequestParamterValues = paramterValues;

            ResponseValueType = IsAsyncRequest && !IsVoidRequest
                ? responseValueType.GetGenericArguments()[0]
                : responseValueType;
        }
    }
}
