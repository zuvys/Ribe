using System;

namespace Ribe.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
        public int Timeout { get; set; }

        public string Version { get; set; }

        public string Group { get; set; }

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
