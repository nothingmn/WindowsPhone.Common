using System.Device.Location;
using Exercisr.Domain.Exercise;
using Exercisr.Domain.Geo;
using Exercisr.Domain.RunKeeper;
using Exercisr.Domain.RunKeeper.v1;
using Ninject.Modules;
using Exercisr.Contracts.Exercise;
using Exercisr.Contracts.Services;

namespace Exercisr.DI.Modules
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


                Bind<IAccount>().To<Account>().InSingletonScope();//.BindingConfiguration.IsImplicit = true;
                //Bind<IAccount>().To<WindowsPhone.Data.DTO.Account>().Named("DTO");

                Bind<IPublishActivity>().To<RunKeeperActivityPublisher>();

                Bind<IActivity>().To<Activity>();
                Bind<IPath>().To<Path>();

                Bind<IHistory>().To<History>().InSingletonScope();
                Bind<IHistoryItem>().To<HistoryItem>();
                Bind<IExerciseType>().To<ExerciseType>();


            }
        }
    }
}