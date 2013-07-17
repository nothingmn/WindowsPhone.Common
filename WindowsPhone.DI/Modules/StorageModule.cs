using Ninject.Modules;
using Windows.Storage;
using WindowsPhone.Contracts.Storage;
using WindowsPhone.Storage;

namespace WindowsPhone.DI.Modules
{
    internal class StorageModule : NinjectModule
    {
        public override void Load()
        {
            {

                //Application level stuff, config, etc..
                Bind<IStorageFolder>().ToMethod(ctx =>
                    {
                        return Windows.Storage.ApplicationData.Current.LocalFolder;
                    });
                Bind<DataStorageHelper>().To<DataStorageHelper>();
                Bind<IGroup>().To<Group>();
                Bind<IItem>().To<Item>();

            }
        }
    }
}