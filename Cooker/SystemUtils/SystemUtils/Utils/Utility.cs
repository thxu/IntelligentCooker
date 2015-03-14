using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Drawing;
namespace Cooker
{
    namespace SystemUtils
    {
        public class Utility
        {
            public static string GetApplicationFullName()
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
            //返回主程序的绝对路径，不包括文件名，包括目录结束的'\'
            public static string GetApplicationAbsolutePath()
            {
                string appName = Process.GetCurrentProcess().MainModule.FileName;
                return Path.GetDirectoryName(appName) + "\\";
            }
        }
    }
}
