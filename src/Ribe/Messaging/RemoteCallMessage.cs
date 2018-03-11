﻿using System.Collections.Generic;

namespace Ribe.Messaging
{
    public class RemoteCallMessage
    {
        public Dictionary<string, string> Headers { get; set; }

        public object[] ParamterValues { get; set; }

        public RemoteCallMessage(Dictionary<string, string> headers, object[] paramterValues)
        {
            Headers = headers;
            ParamterValues = paramterValues;
        }
    }
}
