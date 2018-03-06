using System.Collections.Generic;
using System.Linq;

namespace Ribe.Client
{
    public class RpcServiceProxyOption : Dictionary<string, string>
    {
        public RpcServiceProxyOption()
        {
            this[Constants.Group] = Constants.DefaultGroup;
            this[Constants.Version] = Constants.DefaultVersion;
        }

        public RpcServiceProxyOption Clone(IEnumerable<KeyValuePair<string, string>> kvs = null)
        {
            var options = new RpcServiceProxyOption();

            foreach (var option in this.Concat(kvs))
            {
                options[option.Key] = option.Value;
            }

            return options;
        }
    }
}
