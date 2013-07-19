using Ninject.Modules;
using RunKeeper8.Contracts.ViewModels;
using RunKeeper8.Domain.ViewModels;

namespace RunKeeper8.DI.Modules
{
    internal class DomainModelsModule : NinjectModule
    {
        public override void Load()
        {
            {

                //Application specific domain model binding
                Bind<ITrackingViewModel>().To<TrackingViewModel>();
                Bind<IOAuthViewModel>().To<ServiceAccountAuthWebViewModel>();

            }
        }
    }
}