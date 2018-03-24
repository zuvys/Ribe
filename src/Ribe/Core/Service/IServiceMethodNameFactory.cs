using System.Reflection;

namespace Ribe.Core.Service
{
    public interface IServiceMethodNameFactory
    {
        string CreateName(MethodInfo method);
    }
}
