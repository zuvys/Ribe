using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Core
{
    public class Request
    {
        public byte[] Body { get; }

        public long RequestId { get; }

        public Header Header { get; }

        public string ServicePath => Header.GetValueOrDefault(Constants.ServicePath);

        public string ServiceMethodName => Header.GetValueOrDefault(Constants.MethodName);

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
