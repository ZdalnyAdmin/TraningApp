using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AppEngine.Helpers
{
    public class Log : HttpServerUtilityBase
    {
        private enum LogType
        {
            INFO = 0,
            WARNING = 1,
            ERROR = 2
        }

        private const int MAX_LOG_SIZE = 1048576; //1MB
        private static TextWriter logTW;
        private static string logPath;
        private static object lockObj = new object();

        static Log()
        {
            logPath = HostingEnvironment.MapPath(Path.Combine("~/Log/", string.Format("log-{0}.txt",DateTime.Now.ToString("ddMMyyyyHHmmss"))));

            if (!Directory.Exists(HostingEnvironment.MapPath("~/Log/")))
            {
                Directory.CreateDirectory(HostingEnvironment.MapPath("~/Log/"));
            }

            logTW = TextWriter.Synchronized(File.AppendText(logPath)); 
        }

        public static void Info(string message)
        {
            AddToLog(message, LogType.INFO);
        }

        public static void Warning(string message)
        {
            AddToLog(message, LogType.WARNING);
        }

        public static void Error(string message)
        {
            AddToLog(message, LogType.ERROR);
        }

        private static void AddToLog(string message, LogType type)
        {
            var logDate = DateTime.Now;
            var logType = string.Empty;

            lock (lockObj)
            {
                FileInfo f = new FileInfo(logPath);
                if (f.Length >= MAX_LOG_SIZE)
                {
                    logPath = HostingEnvironment.MapPath(Path.Combine("~/Log/", string.Format("log-{0}.txt", DateTime.Now.ToString("ddMMyyyyHHmmss"))));
                    logTW.Close();
                    logTW = TextWriter.Synchronized(File.AppendText(logPath));
                }
            }

            switch (type)
            {
                case LogType.ERROR:
                    logType = "ERROR";
                    break;

                case LogType.WARNING:
                    logType = "WARNING";
                    break;

                case LogType.INFO:
                default:
                    logType = "INFO";
                    break;
            }

            var logMessage = string.Format("{0} {1}: {2}",
                                            logDate.ToString("dd/MM/yyyy HH:mm:ss"),
                                            logType,
                                            message);

            logTW.WriteLine(logMessage);
            logTW.Flush();
        }
    }
}
