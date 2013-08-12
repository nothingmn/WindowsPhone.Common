using Exercisr.Domain.ViewModels;
using Ninject.Modules;
using Exercisr.Contracts.ViewModels;

namespace Exercisr.DI.Modules
{
    internal class DomainModelsModule : NinjectModule
    {
        public override void Load()
        {
            {

                //Application specific domain model binding
                Bind<ITrackingViewModel>().To<TrackingViewModel>();
                Bind<IOAuthViewModel>().To<ServiceAccountAuthWebViewModel>();
                Bind<IHomeViewModel>().To<HomeViewModel>();

            }
        }
    }
}