using System;

namespace Ribe.Core
{
    /// <summary>
    /// used for class means a service impl,used for interface means a client definition for service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
        public int Timeout { get; set; }

        public string Group { get; set; }

        public string Version { get; set; }

        //Route|Path|etc

        public ServiceAttribute()
        {
            if (String.IsNullOrEmpty(Version))
            {
                Version = "0.0.0";
            }

            if (Group == null)
            {
                Group = "default";
            }
        }
    }
}
