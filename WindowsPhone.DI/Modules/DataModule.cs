using Ninject.Modules;
using Windows.Storage;
using WindowsPhone.Contracts.Membership;
using WindowsPhone.Contracts.Repository;
using WindowsPhone.Data;
using WindowsPhone.Data.DTO;

namespace WindowsPhone.DI.Modules
{
    internal class DataModule : NinjectModule
    {
        public override void Load()
        {
            {

                Bind<IRepository>().To<SqliteRepository>();


                Bind<IUser>().To<User>().Named("DTO");

            }
        }
    }
}