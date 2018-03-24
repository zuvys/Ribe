namespace Ribe.Core.Service
{
    public interface IServiceProvider
    {
        ServiceEntry GetEntry(Request req);
    }
}
