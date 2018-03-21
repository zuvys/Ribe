using System;
using System.Collections.Generic;
using System.Text;

namespace Ribe.Rpc.Logging
{
    public interface ILogger
    {
        bool IsEnabled(LogLevel level);

        void Log(LogLevel level, string log, Exception e);
    }
}
