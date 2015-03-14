using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
namespace Cooker
{
    namespace SystemUtils
    {
        internal class InternalLogger
        {
            public static void LogException(Exception e)
            {
                Logger.LogException(e);
            }
            public static void LogError(string name)
            {
                Logger.LogError(FMainSource,name);
            }
            public static void LogWarning(string name)
            {
                Logger.LogWarning(FMainSource,name);
            }
            public static void LogMessage(string name)
            {
                Logger.LogMessage(FMainSource,name);
            }
            private const string FMainSource = "SystemUtils";

        }
    }
}
