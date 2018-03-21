using System.Collections.Generic;
using System.Linq;

namespace Ribe.Core
{
    public class RequestHeader : Dictionary<string, string>
    {
        public RequestHeader()
        {
            this[Constants.Group] = Constants.DefaultGroup;
            this[Constants.Accept] = Constants.DefaultAccpet;
            this[Constants.Version] = Constants.DefaultVersion;
            this[Constants.ContentType] = Constants.DefaultContentType;
        }

        public RequestHeader Clone(IEnumerable<KeyValuePair<string, string>> kvs = null)
        {
            var options = new RequestHeader();

            foreach (var option in this.Concat(kvs))
            {
                options[option.Key] = option.Value;
            }

            return options;
        }
    }
}
