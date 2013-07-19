using RunKeeper8.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunKeeper8.Domain.RunKeeper.v1
{
    public class Account : IAccount
    {

        public Account()
        {
            ClientID = "67778de52c0442e296e3ed149fec3af3";
            ClientSecret = "0a0cfce7c10c4a5b9e7d2eb9342083a9";
            AuthorizationURL = "https://runkeeper.com/apps/authorize";
            AccessTokenURL = "https://runkeeper.com/apps/token";
            DeAuthorizationURL = "https://runkeeper.com/apps/de-authorize";
            RedirectURL = "http://robchartier.ca";
        }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationURL { get; set; }
        public string AccessTokenURL { get; set; }
        public string DeAuthorizationURL { get; set; }
        public string RedirectURL { get; set; }
        public string Code { get; set; }
        public string AccessToken { get; set; }

        public string AuthorizationEndPoint()
        {
            string url = AuthorizationURL;
            if (!url.Contains("?"))
            {
                url = url + "?";
            }
            else
            {
                if (!url.EndsWith("&")) url = url + "&";
            }
            
            url = url + "client_id=" + System.Uri.EscapeUriString(ClientID);
            url = url + "&response_type=code";
            url = url + "&redirect_uri=" + System.Uri.EscapeUriString(RedirectURL);
            return url;

        }

        public string TokenEndPoint()
        {
            string url = AccessTokenURL;
            if (!url.Contains("?"))
            {
                url = url + "?";
            }
            else
            {
                if (!url.EndsWith("&")) url = url + "&";
            }

            url = url + "client_id=" + System.Uri.EscapeUriString(ClientID);
            url = url + "&response_type=code";
            url = url + "&code=" + System.Uri.EscapeUriString(Code);
            url = url + "&client_secret=" + System.Uri.EscapeUriString(ClientSecret);
            url = url + "&redirect_uri=" + System.Uri.EscapeUriString(RedirectURL);
            return url;
        }        
    }
}
