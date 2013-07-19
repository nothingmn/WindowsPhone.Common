using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.Services;
using RunKeeper8.Contracts.ViewModels;
using WindowsPhone.Common.ViewModels;

namespace RunKeeper8.Domain.ViewModels
{
    public class ServiceAccountAuthWebViewModel : ViewModelBase, IOAuthViewModel 
    {

        public ServiceAccountAuthWebViewModel(IAccount account)
        {
            ServiceAccount = account;
        }

        private IAccount _serviceAccount;
        public IAccount ServiceAccount { get { return _serviceAccount; } set { _serviceAccount = value; base.OnPropertyChanged("ServiceAccount"); } }

        private string _Url;
        public string Url { get { return _Url; } set { _Url = value; base.OnPropertyChanged("Url"); } }
    }
}
