using System.Collections.Generic;

namespace Ribe.Client
{
    public class ServiceProxyOption : Dictionary<string, string>
    {
        public ServiceProxyOption()
        {
            this[Constants.Group] = Constants.DefaultGroup;
            this[Constants.Version] = Constants.DefaultVersion;
        }
    }
}
