using System.Windows.Navigation;
using Ninject.Modules;
using WindowsPhone.Common;
using WindowsPhone.Common.Localization;
using WindowsPhone.Common.Membership;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Core;
using WindowsPhone.Contracts.Localization;
using WindowsPhone.Contracts.Membership;
using WindowsPhone.Contracts.Navigation;
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
                Bind<ILocalize>().To<SimpleLocalizer>().Named("Simple");
                Bind<IApplication>().To<Application>().InSingletonScope();

                Bind<INavigationService>().To<WindowsPhone.Common.Navigation.NavigationService>().InSingletonScope();
                Bind<IUser>().To<User>().BindingConfiguration.IsImplicit = true;


            }
        }
    }
}