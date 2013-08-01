using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RunKeeper8.Contracts.ViewModels;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts.Navigation;

namespace RunKeeper8
{
    public partial class Home : PhoneApplicationPage
    {
        private IHomeViewModel model;
        public Home()
        {
            InitializeComponent();



            this.Loaded += Home_Loaded;
        }

        void Home_Loaded(object sender, RoutedEventArgs e)
        {
            INavigationService nav = DI.Container.Current.Get<INavigationService>();
            nav.SetCurrentService(NavigationService);
            model = DI.Container.Current.Get<IHomeViewModel>();
            (model as ViewModelBase).Attach(this);
            LayoutRoot.DataContext  = model;

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            model.ExerciseCommand.Execute(e);

        }

    }
}