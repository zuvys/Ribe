using System.Collections.Generic;
using System.Linq;

namespace Ribe.Client
{
    public class ServiceProxyOption : Dictionary<string, string>
    {
        public ServiceProxyOption()
        {
            this[Constants.Group] = Constants.DefaultGroup;
            this[Constants.Accept] = Constants.DefaultAccpet;
            this[Constants.Version] = Constants.DefaultVersion;
            this[Constants.ContentType] = Constants.DefaultContentType;
        }

        public ServiceProxyOption Clone(IEnumerable<KeyValuePair<string, string>> kvs = null)
        {
            var options = new ServiceProxyOption();

            foreach (var option in this.Concat(kvs))
            {
                options[option.Key] = option.Value;
            }

            return options;
        }
    }
}
