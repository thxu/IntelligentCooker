using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Cooker
{
    namespace SystemUtils
    {
        //日志级别
        public enum LogLevel { llMessage, llWarning, llError };
        public delegate void LogNotifyEvent(LogEvent log); 
        // 日志信息结构
        public struct LogEvent
        {
            public LogLevel Level;  // 消息级别
            public DateTime Time;   // 消息发生时间
            public string Source;   // 消息源
            public string Message;  // 事件内容
            public Int32 ThreadId;  // 线程ID
        };
        // 日志记录参数类，用于OpenLogger的参数定义。
        // 此类用于屏蔽文件日志和网络日志等记录方式的区别
        public class LoggerArg
        {
            // 定义文件日志方式，日志记录到名为fileName的文件中
            public LoggerArg(string fileName)
            {
                FArgType = LoggerArgType.latFile;
                FFileName = fileName;
                FbNoBuffer = false;
            }
            public LoggerArg(string fileName, bool bNoBuffer)
            {
                FArgType = LoggerArgType.latFile;
                FFileName = fileName;
                FbNoBuffer = bNoBuffer;
            }
            public enum LoggerArgType { latFile, latNet };
            public LoggerArgType ArgType
            {
                get { return FArgType; }
            }
            
            public string FileName
            {
                get { return FFileName; }
            }
            public bool NoBuffer
            {
                get { return FbNoBuffer; }
            }
            private LoggerArgType FArgType;
            private string FFileName;
            private bool FbNoBuffer;
        };
        public class Logger
        {
            public static void LogMessage(string src, string msg)
            {
                if(CurrentLogger != null)
                    CurrentLogger.LogMessage(src, msg);
            }
            public static void LogWarning(string src, string msg)
            {
                if (CurrentLogger != null)
                    CurrentLogger.LogWarning(src, msg);
            }
            public static void LogError(string src, string msg)
            {
                if (CurrentLogger != null)
                    CurrentLogger.LogError(src, msg);
            }
            public static void LogException(Exception e)
            {
                if (CurrentLogger != null)
                    CurrentLogger.LogException(e);
            }
            public static void LogException(Exception e, string file, int line)
            {
                if (CurrentLogger != null)
                    CurrentLogger.LogException(e, file, line);
            }
            public static void LogOSError()
            {
                if (CurrentLogger != null)
                    CurrentLogger.LogOSError();
            }
            public static void LogAction(long userID, string action, string description, string logSrc, DateTime logtime)
            {
                if (CurrentLogger != null)
                    CurrentLogger.LogAction(userID, action, description, logSrc, logtime);
            }
            public static bool OpenLogger(ref LoggerArg arg)
            {
                if( CurrentLogger != null )
                {
                    OpenCount++;
                    return true;
                }
                if( arg.ArgType == LoggerArg.LoggerArgType.latFile)
                {
                    try
                    {
                        CurrentLogger = new FileLogger(arg.FileName, arg.NoBuffer);
                        OpenCount = 1;
                        return true;
                    }
                    catch
                    {
                        CurrentLogger.Dispose();
                        OpenCount = 0;                        
                        return false;
                    }
                }
                else // 其它类型还不支持
                {
                    return false;
                }                  
            }
            public static void CloseLogger()
            {
                if (OpenCount > 0)
                {
                    OpenCount--;
                    if (OpenCount == 0)
                        CurrentLogger.Dispose();
                }
            }
            public static LogNotifyEvent RegisterLogNotifyEvent(LogNotifyEvent logEvent)
            {
                if( CurrentLogger == null) 
                    return null;
                LogNotifyEvent Event = CurrentLogger.OnLogChanged;
                CurrentLogger.OnLogChanged = logEvent;
                return Event;
            }
            private static AbstractLogger CurrentLogger = null; 
            private static int OpenCount=0;
        }
        
    }
}
