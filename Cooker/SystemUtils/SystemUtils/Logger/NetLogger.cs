using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Cooker
{
    namespace SystemUtils
    {
            internal class NetLogger : AbstractLogger
            {
                public NetLogger()
                {

                }
                public override void Dispose() { }
                protected override void LogEvent(LogLevel level, string src, string msg)
                {

                }
                public override void LogException(Exception e)
                {

                }
                public override void LogException(Exception e, string file, int line)
                {

                }
                public override void LogMessage(string src, string msg)
                {
                    LogEvent(LogLevel.llMessage, src, msg);
                }
                public override void LogError(string src, string msg)
                {
                    LogEvent(LogLevel.llError, src, msg);
                }
                public override void LogWarning(string src, string msg)
                {
                    LogEvent(LogLevel.llWarning, src, msg);
                }
                public override void LogOSError()
                {
                    
                }
                public override void LogAction(long userID, string action, string description, string logSrc, DateTime logtime)
                {

                }
            }
        }
}