using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using RunKeeper8.Contracts.Excercise;
using RunKeeper8.Contracts.Geo;
using RunKeeper8.Contracts.Services;
using RunKeeper8.Contracts.ViewModels;
using RunKeeper8.Domain.Excercise;
using RunKeeper8.Domain.Geo;
using WindowsPhone.Common.Command;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Localization;
using WindowsPhone.Contracts.Logging;
using System.ComponentModel;
using WindowsPhone.Contracts.Navigation;

namespace RunKeeper8.Domain.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHomeViewModel, INotifyPropertyChanged
    {


        private ILog _log;
        private ILocalize _localize;
        private IApplication _application;
        public IAccount Account { get; set; }
        private IHistory _history;
        private INavigationService _NavigationService;
        public HomeViewModel(ILog log, IAccount account, ILocalize localize, IApplication application, IHistory history, INavigationService navigationService)
        {
            _log = log;
            _localize = localize;
            _application = application;
            Account = account;

            _history = history;
            _NavigationService = navigationService;

        }


        public string AppTitle
        {
            get { return _application.Name; }
            set
            {
                _application.Name = value;
                OnPropertyChanged("AppTitle");
            }
        }


        public IHistory History
        {
            get { return _history; }
            set
            {
                _history = value;
                OnPropertyChanged("History");
            }
        }

        public ICommand BookmarkCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                    });
            }
        }

        public ICommand RunningCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Running", UriKind.RelativeOrAbsolute));
                    });
            }
        }

        public ICommand WalkingCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Walking", UriKind.RelativeOrAbsolute));
                    });
            }
        }

        public ICommand CyclingCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Cycling", UriKind.RelativeOrAbsolute));
                    });
            }
        }

        public ICommand MountainBikingCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Mountain Biking", UriKind.RelativeOrAbsolute));

                    });
            }
        }

        public ICommand HikingCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Hiking", UriKind.RelativeOrAbsolute));

                    });
            }
        }

        public ICommand DownhillSkiingCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Downhill+Skiing", UriKind.RelativeOrAbsolute));

                    });
            }
        }

        public ICommand CrossCountrySkiingCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Crosscountry+Skiing", UriKind.RelativeOrAbsolute));

                    });
            }
        }

        public ICommand SnowboardingCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Snowboarding", UriKind.RelativeOrAbsolute));

                    });
            }
        }

        public ICommand WheelchairCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Wheelchair", UriKind.RelativeOrAbsolute));

                    });
            }
        }

        public ICommand OtherCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        //navigate to the bookmark interface
                        _NavigationService.Navigate(new Uri("/MainPage.xaml?type=Other", UriKind.RelativeOrAbsolute));

                    });
            }
        }

    }
}
