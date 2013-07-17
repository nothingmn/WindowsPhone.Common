using System;
using Microsoft.Phone.Logging;
using WindowsPhone.Contracts.Logging;

namespace WindowsPhone.Logging
{
    public class NLogLogger :WindowsPhone.Contracts.Logging.ILog
    {
        private ILog _impl;
        private string _loggerType = typeof (NLogLogger).FullName;

        public NLogLogger(Type T)
        {
            _loggerType = T.FullName;
            _impl = new DebugLogger(T);
        }

        public NLogLogger(string Name)
        {
            _loggerType = Name;
            _impl = new DebugLogger(Name);
        }

        public void Debug(object message)
        {
            _impl.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            _impl.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _impl.DebugFormat(format, args);
        }

        public void Info(object message)
        {
            _impl.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _impl.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _impl.InfoFormat(format, args);
        }

        public void Warn(object message)
        {
            _impl.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _impl.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _impl.WarnFormat(format, args);
        }

        public void Error(object message)
        {
            _impl.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _impl.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _impl.ErrorFormat(format, args);
        }

        public void Fatal(object message)
        {
            _impl.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _impl.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _impl.FatalFormat(format, args);
        }

        public bool IsDebugEnabled
        {
            get { return _impl.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _impl.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _impl.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _impl.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _impl.IsFatalEnabled; }
        }
    }
}