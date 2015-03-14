using System;
using System.Reflection;
namespace Cooker
{

    namespace SystemUtils
    {
            internal abstract class AbstractLogger
            {
                protected abstract void LogEvent(LogLevel level, string src, string msg);
                public abstract void Dispose();
                public abstract void LogException(Exception e);
                public abstract void LogException(Exception e, string file, int line);
                public abstract void LogOSError();
                public abstract void LogMessage(string src, string msg);
                public abstract void LogWarning(string src, string msg);
                public abstract void LogError(string src, string msg);

                public abstract void LogAction(long userID, string action, string description, string logSrc, DateTime logtime);
                public LogNotifyEvent OnLogChanged
                { 
                    get 
                    {
                        return FOnLogChanged;
                    }
                    set
                    {
                        FOnLogChanged = value;
                    }
                }

                private LogNotifyEvent FOnLogChanged = null;
            }
    }
}
