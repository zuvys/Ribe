using System.Reflection;

namespace Ribe.Rpc.Core.Service
{
    public interface IServiceMethodNameFactory
    {
        string CreateName(MethodInfo method);
    }
}
