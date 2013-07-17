using Ninject.Modules;
using WindowsPhone.Common;
using WindowsPhone.Contracts.Core;
using WindowsPhone.Contracts.Serialization;
using WindowsPhone.Serialization.JSON;

namespace WindowsPhone.DI.Modules
{
    internal class CommonModule : NinjectModule
    {
        public override void Load()
        {
            {

                //Common models
                Bind<IVersion>().To<AppVersion>();
                Bind<ISerialize>().To<JSONSerializer>().Named("JSON");

            }
        }
    }
}