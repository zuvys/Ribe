﻿using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Core.Service
{
    public class ServiceEntry
    {
        public Type ServiceType { get; set; }

        public string ServiceName { get; set; }

        public string ServicePath { get; set; }

        public ServiceAttribute Attribute { get; set; }

        public Dictionary<string, ServiceMethod> Methods { get; set; }
    }
}
