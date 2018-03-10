using System;
using System.Collections.Generic;
using System.Text;

namespace Ribe.Client.Extensions
{
    public static class ServiceOptionExtensions
    {
        public static ServiceProxyOption WithGroup(this ServiceProxyOption options, string group)
        {
            options[Constants.Group] = group;

            return options;
        }

        public static ServiceProxyOption WithVersion(this ServiceProxyOption options, string version)
        {
            options[Constants.Version] = version;

            return options;
        }
    }
}
