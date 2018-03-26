using Ribe.Rpc.Core.Service;
using System;

namespace Ribe.Rpc.Core.Executor
{
    public class ExecutionContext
    {
        public Type ServiceType { get; set; }

        public ServiceMethod ServiceMethod { get; set; }

        public object[] ParamterValues { get; set; }
    }
}
