using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts.Logging;

namespace WindowsPhone.Logging
{
    public class DebugLogger : ILog
    {

        public DebugLogger(Type T)
        {
            TypeName = T.FullName;

        }

        public DebugLogger(string Name)
        {
            TypeName = Name;
        }

        public string TypeName { get; set; }
        private const string ErrorText = "ERROR";
        private const string WarnText = "WARN";
        private const string InfoText = "INFO";
        private const string DebugText = "DEBUG";
        private const string FatalText = "FATAL";

        private void WriteLogMessage(string Level, string format = "{0}", params object[] args)
        {
            System.Diagnostics.Debug.WriteLine("[{0}] {1} {2}", Level, DateTime.Now.ToString("o"),
                                               string.Format(format, args));
        }

        public void Debug(object message)
        {
            WriteLogMessage(DebugText, "{0}", message);
        }

        public void Debug(object message, Exception exception)
        {
            WriteLogMessage(DebugText, "{0}-{1}", message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            WriteLogMessage(DebugText, format, args);
        }

        public void Info(object message)
        {
            WriteLogMessage(InfoText, "{0}", message);
        }

        public void Info(object message, Exception exception)
        {
            WriteLogMessage(InfoText, "{0}-{1}", message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            WriteLogMessage(InfoText, format, args);
        }

        public void Warn(object message)
        {
            WriteLogMessage(WarnText, "{0}", message);
        }

        public void Warn(object message, Exception exception)
        {
            WriteLogMessage(WarnText, "{0}-{1}", message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            WriteLogMessage(WarnText, format, args);
        }

        public void Error(object message)
        {
            WriteLogMessage(ErrorText, "{0}", message);
        }

        public void Error(object message, Exception exception)
        {
            WriteLogMessage(ErrorText, "{0}-{1}", message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            WriteLogMessage(ErrorText, format, args);
        }

        public void Fatal(object message)
        {
            WriteLogMessage(FatalText, "{0}", message);
        }

        public void Fatal(object message, Exception exception)
        {
            WriteLogMessage(FatalText, "{0}-{1}", message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            WriteLogMessage(FatalText, format, args);
        }

#if DEBUG
        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

#else
        public bool IsDebugEnabled
        {
            get { return false; }
        }

        public bool IsInfoEnabled
        {
            get { return false; }
        }

        public bool IsWarnEnabled
        {
            get { return false; }
        }
#endif

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }

    }
}