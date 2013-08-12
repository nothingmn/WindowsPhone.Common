using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Exercisr.Contracts.Services;
using WindowsPhone.Contracts.Communication.Http;
using System.IO;

namespace Exercisr.Domain.RunKeeper
{

    public class RunKeeperActivityPublisher : IPublishActivity
    {
        public event PublishComplete OnPublishComplete;
        public IAccount _Account;
        bool CanPublish { get { return !string.IsNullOrEmpty(_Account.AccessToken); } }
        public RunKeeperActivityPublisher(IAccount account)
        {
            _Account = account;            

        }
        public void Publish(IActivity activity)
        {
            if (CanPublish)
            {
                var http = WindowsPhone.DI.Container.Current.Get<IHttpClient>();
                http.OnHttpDownloaded += http_OnHttpDownloaded;
                http.OnHttpDownloadError += http_OnHttpDownloadError;
                var serializer =
                    WindowsPhone.DI.Container.Current.Get<WindowsPhone.Contracts.Serialization.ISerialize>("JSON");
                var json = serializer.Serialize(activity);

                WebHeaderCollection headers = new WebHeaderCollection();
                headers["Authorization"] = string.Format("Bearer {0}", _Account.AccessToken);
                http.POST(_Account.RecordActivityEndPoint(), System.Text.Encoding.UTF8.GetBytes(json), null, null, null,
                          null, null, "application/vnd.com.runkeeper.NewFitnessActivity+json", true,
                          "application/vnd.com.runkeeper.NewFitnessActivity+json", false, null, headers);
            }
        }

        private void http_OnHttpDownloadError(object Sender, Exception exception, string body, string Key)
        {
            if (OnPublishComplete != null) OnPublishComplete(this, DateTime.Now, false, exception, body);
        }

        void http_OnHttpDownloaded(object Sender, byte[] Data, long Duration, string Key, System.Net.WebHeaderCollection Headers = null)
        {
            string body = System.Text.Encoding.UTF8.GetString(Data, 0, Data.Length);
            if (OnPublishComplete != null) OnPublishComplete(this, DateTime.Now, true, null, body);            
        }

    }
}
