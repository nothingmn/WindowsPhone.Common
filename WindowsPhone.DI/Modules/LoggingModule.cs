using Ninject.Activation;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Logging;

namespace WindowsPhone.DI.Modules
{
    public class LoggingModule : NinjectModule
    {
        public override void Load()
        {

            Bind<ILog>().ToMethod(iContext =>
                {
                    ILogFactory factory = DI.Container.Current.Get<ILogFactory>();
                    return factory.GetLogger(GetParentTypeName(iContext));
                }).InTransientScope();


            Bind<ILogFactory>().To<LogFactory>().InSingletonScope();
        }

        private Type GetParentTypeName(IContext iContext)
        {
            if (iContext.Request.Target == null || iContext.Request.Target.Member == null)
            {
                try
                {
#if DEBUG
                    var frame = new System.Diagnostics.StackTrace(true).GetFrame(11);
                    return frame.GetMethod().DeclaringType;
#else
                    return typeof(ILog);
#endif
                }
                catch (Exception)
                {
                    return typeof (ILog);
                }
            }
            return iContext.Request.Target.Member.ReflectedType;
        }
    }
}
