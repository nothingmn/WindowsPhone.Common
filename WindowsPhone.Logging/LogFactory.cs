using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts.Logging;
using ILog = WindowsPhone.Contracts.Logging.ILog;

namespace WindowsPhone.Logging
{
    public class LogFactory : ILogFactory
    {
        static LogFactory()
        {
        }

        public ILog GetLogger(Type T)
        {
            return new NLogLogger(T);
        }

        public ILog GetLogger(string Name)
        {
            return new NLogLogger(Name);
        }
    }
}
