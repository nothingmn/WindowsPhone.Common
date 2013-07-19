using System.Device.Location;
using Ninject.Modules;
using RunKeeper8.Contracts.ViewModels;
using RunKeeper8.Domain.Geo;
using RunKeeper8.Domain.ViewModels;

namespace RunKeeper8.DI.Modules
{
    internal class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            {

                //Application specific domain model binding
                Bind<IGeoPositionWatcher<GeoCoordinate>>().To<GeoCoordinateSimulator>();

            }
        }
    }
}