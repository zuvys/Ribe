using System;

namespace Ribe.Rpc.Logging
{
    public class NullLogger : ILogger
    {
        public static readonly ILogger Instance = new NullLogger();

        public bool IsEnabled(LogLevel level)
        {
            return false;
        }

        public void Log(LogLevel level,string log, Exception e)
        {

        }
    }
}
