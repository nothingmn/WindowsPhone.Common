using Ninject.Modules;
using WindowsPhone.Common;
using WindowsPhone.Common.Communication;
using WindowsPhone.Common.Communication.Channels;
using WindowsPhone.Common.Communication.Http;
using WindowsPhone.Contracts.Communication;
using WindowsPhone.Contracts.Communication.Http;
using WindowsPhone.Contracts.Core;
using WindowsPhone.Contracts.Serialization;
using WindowsPhone.Serialization.JSON;

namespace WindowsPhone.DI.Modules
{
    internal class CommunicationModule : NinjectModule
    {
        public override void Load()
        {
            {

                //Common models
                Bind<IConnection>().To<BluetoothConnection>();
                Bind<IChannel>().To<ByteArrayChannel>().Named("byte");
                Bind<IChannel>().To<CSVChannel>().Named("csv");
                Bind<IChannel>().To<StringChannel>().Named("string");

                Bind<IHttpClient>().To<HttpClient>();

            }
        }
    }
}