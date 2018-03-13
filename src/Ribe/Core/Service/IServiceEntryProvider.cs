using System;
using System.Collections.Generic;
using System.Text;

namespace Ribe.Core.Service
{
    public interface IServiceEntryProvider
    {
        ServiceEntry GetServiceEntry(ServiceRequestContext context);
    }
}
