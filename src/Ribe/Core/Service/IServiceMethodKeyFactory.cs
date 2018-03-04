using System.Reflection;

namespace Ribe.Core.Service
{
    public interface IServiceMethodKeyFactory
    {
        string CreateMethodKey(MethodInfo method);
    }
}
