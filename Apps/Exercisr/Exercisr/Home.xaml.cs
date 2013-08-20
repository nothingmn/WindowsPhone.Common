using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Exercisr.Contracts.Configuration;
using Exercisr.Contracts.Exercise;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Navigation;
using Exercisr.Contracts.ViewModels;

namespace Exercisr
{
    public partial class Home : PhoneApplicationPage
    {
        private IHomeViewModel model;
        private ViewModelBase viewModelbase;
        private ILog _log;
        public Home()
        {
            InitializeComponent();

            this.Loaded += Home_Loaded;
            _log = DI.Container.Current.Get<ILog>();
        }

        void Home_Loaded(object sender, RoutedEventArgs e)
        {
            INavigationService nav = DI.Container.Current.Get<INavigationService>();
            nav.SetCurrentService(NavigationService);
            model = DI.Container.Current.Get<IHomeViewModel>();
            viewModelbase = (model as ViewModelBase);
            viewModelbase.Attach(this);
            (model.History as ViewModelBase).Attach(this);
            LayoutRoot.DataContext = model;

            ISettings settings = DI.Container.Current.Get<ISettings>();
            settings.OnSettingsChanged += settings_OnSettingsChanged;
        }

        private void settings_OnSettingsChanged(ISettings settings, string propertyName)
        {
            if (propertyName == "IsMetric")
            {
                model.History.HistoryItems.Clear();
                model.History.Load();
            }
        }
        private delegate void NoArgDelegate();

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            model.ExerciseCommand.Execute(e);

        }

        private void MenuItemPostToRunKeeperCommand_OnClick(object sender, RoutedEventArgs e)
        {
            var menu = (sender as MenuItem);
            model.PostToRunKeeperCommand.Execute(menu.CommandParameter);
        }
    }
}