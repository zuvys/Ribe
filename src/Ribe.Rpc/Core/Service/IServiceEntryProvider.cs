using System.Collections.Generic;

namespace Ribe.Rpc.Core.Service
{
    public interface IServiceEntryProvider
    {
        ServiceEntry GetEntry(Request req);

        IEnumerable<ServiceEntry> GetAll();
    }
}
