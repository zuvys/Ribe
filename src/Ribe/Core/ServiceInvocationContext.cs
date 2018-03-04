using Ribe.Messaging;

using System;
using System.Collections.Generic;

namespace Ribe.Core
{
    public class ServiceInvocationContext
    {
        public IMessage Message { get; }

        public Func<Type[], byte[], object[]> ParamterValuesConvertor { get; }

        public string RequestId => Message.Headers.GetValueOrDefault(Constants.RequestId);

        public string ServicePath => Message.Headers.GetValueOrDefault(Constants.ServicePath);

        public string ServiceMethodKey => Message.Headers.GetValueOrDefault(Constants.ServiceMethodKey);

        public ServiceInvocationContext(IMessage message, Func<Type[], byte[], object[]> paramtersValueConvertor)
        {
            Message = message;
            ParamterValuesConvertor = paramtersValueConvertor;
        }
    }
}
