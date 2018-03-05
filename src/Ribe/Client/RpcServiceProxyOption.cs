using System.Collections.Generic;

namespace Ribe.Client
{
    public class RpcServiceProxyOption : Dictionary<string, string>
    {
        public RpcServiceProxyOption()
        {
            this[Constants.Group] = Constants.DefaultGroup;
            this[Constants.Version] = Constants.DefaultVersion;
        }
    }
}
