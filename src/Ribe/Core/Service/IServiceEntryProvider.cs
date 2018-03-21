namespace Ribe.Core.Service
{
    public interface IServiceEntryProvider
    {
        ServiceEntry GetEntry(Request context);
    }
}
