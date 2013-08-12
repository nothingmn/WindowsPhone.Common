using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Exercisr.Contracts.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhone.Common.ViewModels;

namespace Exercisr.UI
{
    public partial class OAuthWebViewPage : PhoneApplicationPage
    {
        private IOAuthViewModel dataContext;
        public OAuthWebViewPage()
        {
            InitializeComponent();
            BrowserControl.Width = this.Width;
            BrowserControl.Height = this.Height;

            dataContext = DI.Container.Current.Get<IOAuthViewModel>();

            this.DataContext = dataContext;

            ((ViewModelBase)dataContext).PropertyChanged += OAuthWebView_PropertyChanged;

            if (!string.IsNullOrEmpty(dataContext.ServiceAccount.AccessToken)) GoHome();

            dataContext.Url = dataContext.ServiceAccount.AuthorizationEndPoint();
        }

        private void GoHome()
        {
            if(NavigationService!=null && NavigationService.CanGoBack) NavigationService.GoBack();
            
        }
        void OAuthWebView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "Url")
            {
                System.Uri u;
                if (System.Uri.TryCreate(dataContext.Url, UriKind.RelativeOrAbsolute, out u))
                {
                    BrowserControl.Navigate(u);
                }
            }
        }

        private void WebBrowser_OnNavigated(object sender, NavigationEventArgs e)
        {
            string url = e.Uri.ToString();
            //dataContext.Url = e.Uri.ToString();
            if (url.StartsWith(dataContext.ServiceAccount.RedirectURL))
            {
                var q = e.Uri.Query;
                if (q.StartsWith("?")) q = q.Substring(1);
                q = q.Replace("code=", "");
                dataContext.UpdateAccessCode(q);
                GoHome();
            }
        }
        //https://runkeeper.com/apps/authorize?scope=&response_type=code&redirect_uri=https://authcomplete/&state=&client_id=67778de52c0442e296e3ed149fec3af3
        //https://authcomplete/?code=2856f39210be413b9097c4d346e1ed21


    }



}