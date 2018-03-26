using System.Collections.Generic;
using System.Linq;

namespace Ribe.Rpc.Core
{
    public class Header : Dictionary<string, string>
    {
        public Header()
        {
            this[Constants.Group] = Constants.DefaultGroup;
            this[Constants.Accept] = Constants.Json;
            this[Constants.Version] = Constants.DefaultVersion;
            this[Constants.ContentType] = Constants.DefaultContentType;
        }

        public Header Clone()
        {
            var options = new Header();

            foreach (var option in this)
            {
                options[option.Key] = option.Value;
            }

            return options;
        }
    }
}
