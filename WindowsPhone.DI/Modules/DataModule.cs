using Ninject.Modules;
using Windows.Storage;
using WindowsPhone.Common.Membership;
using WindowsPhone.Contracts.Membership;
using WindowsPhone.Contracts.Repository;
using WindowsPhone.Data;

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