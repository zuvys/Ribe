using Ribe.Core.Service;
using Ribe.Messaging;
using System;
using System.Collections.Generic;

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
