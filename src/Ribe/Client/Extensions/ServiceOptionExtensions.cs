using System;
using System.Collections.Generic;
using System.Text;

namespace Ribe.Client.Extensions
{
    public static class ServiceOptionExtensions
    {
        public static RpcServiceProxyOption WithGroup(this RpcServiceProxyOption options, string group)
        {
            options[Constants.Group] = group;

            return options;
        }

        public static RpcServiceProxyOption WithVersion(this RpcServiceProxyOption options, string version)
        {
            options[Constants.Version] = version;

            return options;
        }
    }
}
