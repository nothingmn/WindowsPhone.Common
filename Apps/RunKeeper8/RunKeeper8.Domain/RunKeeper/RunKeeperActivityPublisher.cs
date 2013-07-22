using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.Services;
using WindowsPhone.Contracts.Communication.Http;

namespace RunKeeper8.Domain.RunKeeper
{
    public class RunKeeperActivityPublisher : IPublishActivity
    {
        public IAccount _Account;
        public RunKeeperActivityPublisher(IAccount account)
        {
            _Account = account;

            if(string.IsNullOrEmpty(_Account.AccessToken)) _Account.Load();

        }
        public void Publish(IActivity activity)
        {
            var http = WindowsPhone.DI.Container.Current.Get<IHttpClient>();
            http.OnHttpDownloaded += http_OnHttpDownloaded;
            http.OnHttpDownloadError += http_OnHttpDownloadError;
            var serializer =
                WindowsPhone.DI.Container.Current.Get<WindowsPhone.Contracts.Serialization.ISerialize>("JSON");
            var json = serializer.Serialize(activity);

            WebHeaderCollection headers = new WebHeaderCollection();
            headers["Authorization"] = string.Format("Bearer {0}", _Account.AccessToken);
            http.POST(_Account.RecordActivityEndPoint(), System.Text.Encoding.UTF8.GetBytes(json), null, null, null, null, null, null, true, "application/vnd.com.runkeeper.NewFitnessActivity+json", false, null, headers);
        }

        void http_OnHttpDownloadError(object Sender, Exception exception, string Key)
        {
        }

        void http_OnHttpDownloaded(object Sender, byte[] Data, long Duration, string Key, System.Net.WebHeaderCollection Headers = null)
        {

            
        }

    }
}
