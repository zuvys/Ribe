﻿using System;

namespace Ribe.Rpc.Logging
{
    public static class LoggerExtension
    {
        public static void Debug(this ILogger logger, string log)
        {
            logger.Log(LogLevel.Debug, log, null);
        }

        public static void Debug(this ILogger logger, Exception e)
        {
            logger.Log(LogLevel.Debug, null, e);
        }

        public static void Debug(this ILogger logger, string log, Exception e)
        {
            logger.Log(LogLevel.Debug, log, e);
        }

        public static void Info(this ILogger logger, string log)
        {
            logger.Log(LogLevel.Info, log, null);
        }

        public static void Info(this ILogger logger, Exception e)
        {
            logger.Log(LogLevel.Info, null, e);
        }

        public static void Info(this ILogger logger, string log, Exception e)
        {
            logger.Log(LogLevel.Info, log, e);
        }

        public static void Warn(this ILogger logger, string log)
        {
            logger.Log(LogLevel.Warn, log, null);
        }

        public static void Warn(this ILogger logger, Exception e)
        {
            logger.Log(LogLevel.Warn, null, e);
        }

        public static void Warn(this ILogger logger, string log, Exception e)
        {
            logger.Log(LogLevel.Warn, log, e);
        }

        public static void Error(this ILogger logger, string log)
        {
            logger.Log(LogLevel.Warn, log, null);
        }

        public static void Error(this ILogger logger, Exception e)
        {
            logger.Log(LogLevel.Warn, null, e);
        }

        public static void Error(this ILogger logger, string log, Exception e)
        {
            logger.Log(LogLevel.Warn, log, e);
        }

        public static void Fatal(this ILogger logger, string log)
        {
            logger.Log(LogLevel.Fatal, log, null);
        }

        public static void Fatal(this ILogger logger, Exception e)
        {
            logger.Log(LogLevel.Fatal, null, e);
        }

        public static void Fatal(this ILogger logger, string log, Exception e)
        {
            logger.Log(LogLevel.Fatal, log, e);
        }
    }
}
