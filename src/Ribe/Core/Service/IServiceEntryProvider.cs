using System.Collections.Generic;

namespace Ribe.Core.Service
{
    public interface IServiceEntryProvider
    {
        ServiceEntry GetEntry(Request req);

        IEnumerable<ServiceEntry> GetAll();
    }
}
