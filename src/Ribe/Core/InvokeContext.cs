using Ribe.Messaging;

using System;
using System.Collections.Generic;

namespace Ribe.Core
{
    public class InvokeContext
    {
        public IMessage Message { get; }

        public Func<Type[], byte[], object[]> ParamterValuesConvertor { get; }

        public string RequestId => Message.Headers.GetValueOrDefault(Constants.RequestId);

        public string ServicePath => Message.Headers.GetValueOrDefault(Constants.ServicePath);

        public string ServiceMethodKey => Message.Headers.GetValueOrDefault(Constants.ServiceMethodKey);

        public InvokeContext(IMessage message, Func<Type[], byte[], object[]> paramtersValueConvertor)
        {
            Message = message;
            ParamterValuesConvertor = paramtersValueConvertor;
        }
    }
}
