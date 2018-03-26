using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Service
{
    public class ServiceMethod
    {
        public MethodInfo Method { get; set; }

        public ParameterInfo[] Parameters { get; set; }

        public Type[] ParamterTypes => Parameters.Select(i => i.ParameterType).ToArray();

        public bool IsAsyncMethod => typeof(Task).IsAssignableFrom(Method.ReturnType);
    }
}
