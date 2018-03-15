using System;
using System.Collections.Generic;

namespace Ribe.Core.Service
{
    public class ServiceEntry
    {
        public Type Interface { get; set; }

        public Type Implemention { get; set; }

        public string ServicePath { get; set; }

        public ServiceAttribute Attribute { get; set; }

        public Dictionary<string, ServiceMethod> MethodMap { get; set; }
    }
}
