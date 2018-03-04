using Ribe.Messaging;
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

        public Dictionary<string, ServiceMethod> ServiceMethodMap { get; set; }

        public ServiceMethod GetServiceMethod(ServiceInvocationContext context)
        {
            if (ServiceMethodMap.ContainsKey(context.ServiceMethodKey))
            {
                return ServiceMethodMap[context.ServiceMethodKey];
            }

            return null;
        }
    }
}
