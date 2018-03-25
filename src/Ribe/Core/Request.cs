using System;
using System.Collections.Generic;

namespace Ribe.Core
{
    public class Request
    {
        public byte[] Body { get; }

        public long RequestId { get; }

        public Header Header { get; }

        public string ServiceName => Header.GetValueOrDefault(Constants.ServiceName);

        public string ServiceMethodKey => Header.GetValueOrDefault(Constants.ServiceMethodName);

        public Func<Type[], object[]> ParamterValuesProvider { get; }

        public Request(
            byte[] body,
            Header header,
            Func<Type[], object[]> paramterValuesProvider
        )
        {
            if (!long.TryParse(header.GetValueOrDefault(Constants.RequestId), out var id))
            {
                throw new ArgumentException("RequestId Key must be numberic");
            }

            Body = body;
            Header = header;
            RequestId = id;
            ParamterValuesProvider = paramterValuesProvider;
        }

        public object[] GetRequestParamterValues(Type[] paramterTypes)
        {
            return ParamterValuesProvider(paramterTypes);
        }
    }
}
