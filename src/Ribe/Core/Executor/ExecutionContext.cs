using System;
using Ribe.Core.Service;
using System.Collections.Generic;
using Ribe.Transport;

namespace Ribe.Core.Executor
{
    public class ExecutionContext
    {
        public Type ServiceType { get; set; }

        public ServiceMethod ServiceMethod { get; set; }

        public object[] ParamterValues { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public IMessageSender Sender { get; set; }
    }
}
