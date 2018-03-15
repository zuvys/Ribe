using System;
using System.Collections.Generic;

namespace Ribe.Core
{
    public class Request
    {
        private Func<Type[], object[]> _paramterValuesProvider;

        public Dictionary<string, string> Headers { get; }

        public byte[] Body { get; }

        public string ServicePath => Headers.GetValueOrDefault(Constants.ServicePath);

        public string ServiceMethodKey => Headers.GetValueOrDefault(Constants.ServiceMethodKey);

        public Request(
            byte[] body,
            Dictionary<string, string> headers,
            Func<Type[], object[]> paramterValuesProvider
        )
        {
            Body = body;
            Headers = headers;
            _paramterValuesProvider = paramterValuesProvider;
        }

        public object[] GetRequestParamterValues(Type[] paramterTypes)
        {
            return _paramterValuesProvider(paramterTypes);
        }
    }
}
