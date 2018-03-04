using System;
using System.Linq;
using System.Reflection;

namespace Ribe.Core.Service
{
    public class ServiceMethod
    {
        public MethodInfo Method { get; set; }

        public ParameterInfo[] Parameters { get; set; }

        public Type[] ParamterTypes => Parameters.Select(i => i.ParameterType).ToArray();
    }
}
