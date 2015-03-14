using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Cooker.SystemUtils;
using System.IO;

namespace IntelligentCooker
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                LoggerArg arg = new LoggerArg(Path.ChangeExtension(Utility.GetApplicationFullName(), ".log"));
                Logger.OpenLogger(ref arg);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new formMain());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                Logger.CloseLogger();
            }
        }
    }
}
