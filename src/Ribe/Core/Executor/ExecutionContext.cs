using System;
using Ribe.Core.Service;
using System.Collections.Generic;

namespace Ribe.Core.Executor
{
    public class ServiceExecutionContext
    {
        public Type ServiceType { get; set; }

        public ServiceMethod ServiceMethod { get; set; }

        public object[] ParamterValues { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}
