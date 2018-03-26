using System;
using System.Collections.Generic;

namespace Ribe.Core
{
    /// <summary>
    /// used for class means a service impl,used for interface means a client definition for service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
        public string Group { get; set; } = Constants.DefaultGroup;

        public string Version { get; set; } = Constants.DefaultVersion;

        public Dictionary<string, string> GetDescriptions()
        {
            return new Dictionary<string, string>
            {
                [Constants.Group] = Group,
                [Constants.Version] = Version
            };
        }
    }
}
