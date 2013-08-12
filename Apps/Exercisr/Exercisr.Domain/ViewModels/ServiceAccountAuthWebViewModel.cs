using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercisr.Contracts.Services;
using Exercisr.Contracts.ViewModels;
using Exercisr.Domain.RunKeeper.v1;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts.Communication.Http;

namespace Exercisr.Domain.ViewModels
{
    public class ServiceAccountAuthWebViewModel : ViewModelBase, IOAuthViewModel 
    {

        public ServiceAccountAuthWebViewModel(IAccount account)
        {
            ServiceAccount = account;

        }

        private IAccount _serviceAccount;
        public IAccount ServiceAccount { get { return _serviceAccount; } set { _serviceAccount = value; base.Dispatcher("ServiceAccount"); } }

        private string _Url;
        public string Url { get { return _Url; } set { _Url = value; base.Dispatcher("Url"); } }

        public void UpdateAccessCode(string code)
        {
            _serviceAccount.Code = code;
            var http = WindowsPhone.DI.Container.Current.Get<IHttpClient>();
            var url = _serviceAccount.TokenEndPoint();
            var ps = System.Text.Encoding.UTF8.GetBytes(_serviceAccount.TokenParameters());
            http.OnHttpDownloaded += http_OnHttpDownloaded;
            http.POST(url, ps, null, null, null, null, null, "application/x-www-form-urlencoded", true, "application/x-www-form-urlencoded", false, null, null);
        }

        void http_OnHttpDownloaded(object Sender, byte[] Data, long Duration, string Key, System.Net.WebHeaderCollection Headers = null)
        {

            //var d = System.Text.Encoding.UTF8.GetString(Data, 0, Data.Length);
            var serializer =
                WindowsPhone.DI.Container.Current.Get<WindowsPhone.Contracts.Serialization.ISerialize>("JSON");
            var res = serializer.Deserialize<TokenRequestResponse>(Data);

            this.UpdateAccessToken(res.access_token);
        }
        public void UpdateAccessToken(string token)
        {
            _serviceAccount.AccessToken = token;

            Dispatcher("ServiceAccount");
        }
    }
}
