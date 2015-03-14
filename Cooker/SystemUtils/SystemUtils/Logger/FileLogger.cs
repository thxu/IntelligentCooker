using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace Cooker
{
	namespace SystemUtils
	{
            internal class FileLogger : AbstractLogger
            {

                public FileLogger(string logFile, bool bNoBuffer)
                {
                    FLogFile = logFile;
                    FLogBufferSize = bNoBuffer ? 0 : FLogBufferSize;
                    FLogs = new Queue<LogEvent>();
                }
                public override void Dispose()
                {
                    DumpLog();
                }
                protected override void LogEvent(LogLevel level, string src, string msg)
                {
                    Monitor.Enter(FLogs);
                    if (FLogs.Count > FLogBufferSize)
                    {
                        DumpLog();
                        FLogs.Clear();
                    }
                    LogEvent log;
                    log.Level = level;
                    log.Message = msg;
                    log.Time = DateTime.Now;
                    log.Source = src;
                    log.ThreadId = Thread.CurrentThread.ManagedThreadId;
                    FLogs.Enqueue(log);
                    if( OnLogChanged != null)
                    {
                        try
                        {
                            OnLogChanged(log);
                        }
                        catch
                        { 
                        }
                    }
                    Monitor.Exit(FLogs);
                }
                public override void LogException(Exception e)
                {
                    try
                    {
                        string content = e.StackTrace; 
                        content += " ";
                        content += e.TargetSite;
                        content += e.Source;
                        LogEvent(LogLevel.llError, e.Message, content);

                    }
                    finally
                    {

                    }
                }
                public override void LogException(Exception e, string file, int line)
                {
                    try
                    {
                        string content = Path.GetFileName(file);
                        content += " ";
                        content += line.ToString();
                        LogEvent(LogLevel.llError, e.Message, content);

                    }
                    finally
                    {

                    }
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
                    //try
                    //{
                    //    IOperationLogs operationLog = DataAccess.CreateOperationLogs();
                    //    operationLog.InsertOperationLog(Utility.GetNextIdentifier(PacsGlobal.LogIDSeqName),userID, action, description, logSrc, logtime);
                    //}
                    //catch (Exception e)
                    //{
                    //    Logger.LogException(e);
                    //}
                }
                public string GetLogFile()
                {
                    Monitor.Enter(FLogFile);
                    string FileName = FLogFile;
                    Monitor.Exit(FLogFile);
                    return FileName;
                }
                private void DumpLog()
                {
                    try
                    {
                        StreamWriter sw = null;
                        if (!File.Exists(FLogFile))
                            sw = File.CreateText(FLogFile);
                        else
                            sw = File.AppendText(FLogFile);
                        foreach (LogEvent logevent in FLogs)
                        {
                            sw.Write(((int)logevent.Level).ToString());
                            sw.Write(' ');
                            sw.Write(logevent.Time.ToString("yyyy-MM-dd HH:mm:ss"));
                            sw.Write(' ');
                            sw.Write(logevent.ThreadId);
                            sw.Write(' ');
                            sw.Write(logevent.Source);
                            sw.Write(' ');
                            sw.WriteLine(logevent.Message);
                        }
                        sw.Close();
                    }
                    finally
                    {

                    }
                }
                private string FLogFile;
                public string LogFile
                {
                    get
                    {
                        Monitor.Enter(FLogFile);
                        string FileName = FLogFile;
                        Monitor.Exit(FLogFile);
                        return FileName;
                    }
                }
                private Queue<LogEvent> FLogs;
                internal uint FLogBufferSize = 200;
            }
        }
}
