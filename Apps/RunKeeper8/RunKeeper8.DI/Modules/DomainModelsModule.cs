using System.Device.Location;
using Ninject.Modules;
using RunKeeper8.Contracts.Services;
using RunKeeper8.Contracts.ViewModels;
using RunKeeper8.Domain.Geo;
using RunKeeper8.Domain.RunKeeper.v1;
using RunKeeper8.Domain.ViewModels;

namespace RunKeeper8.DI.Modules
{
    internal class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            {

                //Application specific domain model binding
#if DEBUG
                Bind<IGeoPositionWatcher<GeoCoordinate>>().To<GeoCoordinateSimulator>();
#else
                Bind<IGeoPositionWatcher<GeoCoordinate>>().To<GeoCoordinateWatcher>();

#endif

                Bind<IAccount>().To<Account>();

            }
        }
    }
}