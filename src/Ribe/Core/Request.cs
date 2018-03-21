using System;
using System.Collections.Generic;

namespace Ribe.Core
{
    public class Request
    {
        public byte[] Body { get; }

        public long RequestId { get; }

        public Dictionary<string, string> Headers { get; }

        public string ServicePath => Headers.GetValueOrDefault(Constants.ServicePath);

        public string ServiceMethodKey => Headers.GetValueOrDefault(Constants.ServiceMethodKey);

        public Func<Type[], object[]> ParamterValuesProvider { get; }

        public Request(
            byte[] body,
            Dictionary<string, string> headers,
            Func<Type[], object[]> paramterValuesProvider
        )
        {
            if (!long.TryParse(headers.GetValueOrDefault(Constants.RequestId), out var id))
            {
                throw new ArgumentException("RequestId Key must be numberic");
            }

            Body = body;
            Headers = headers;
            RequestId = id;
            ParamterValuesProvider = paramterValuesProvider;
        }

        public object[] GetRequestParamterValues(Type[] paramterTypes)
        {
            return ParamterValuesProvider(paramterTypes);
        }
    }
}
