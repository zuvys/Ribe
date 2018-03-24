using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core
{
    public class RequestContext
    {
        public Type ResponseValueType { get; }

        public object[] RequestParamterValues { get; }

        public Dictionary<string, string> RequestHeaders { get; }

        public bool IsVoidRequest { get; }

        public bool IsAsyncRequest { get; }

        public RequestContext(Dictionary<string, string> headers, object[] paramterValues, Type responseValueType)
        {
            IsVoidRequest = responseValueType == null || typeof(Task) == responseValueType || typeof(void) == responseValueType;
            IsAsyncRequest = responseValueType != null && typeof(Task).IsAssignableFrom(responseValueType);

            RequestHeaders = headers;
            RequestParamterValues = paramterValues;

            ResponseValueType = IsAsyncRequest && !IsVoidRequest
                ? responseValueType.GetGenericArguments()[0]
                : responseValueType;
        }
    }
}
