using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Exercisr.Contracts.Exercise;
using Exercisr.Contracts.Services;
using Exercisr.Contracts.ViewModels;
using Exercisr.Domain.Exercise;
using WindowsPhone.Common.Command;
using WindowsPhone.Common.Membership;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Localization;
using WindowsPhone.Contracts.Logging;
using System.ComponentModel;
using WindowsPhone.Contracts.Membership;
using WindowsPhone.Contracts.Navigation;
using WindowsPhone.Contracts.Repository;

namespace Exercisr.Domain.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHomeViewModel, INotifyPropertyChanged
    {

        private ILog _log;
        private ILocalize _localize;
        private IApplication _application;
        public IAccount Account { get; set; }

      
        private IHistory _history;
        private INavigationService _NavigationService;

        private IUser _user;

        public IUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                Dispatcher("User");
            }
        }

        private IList<IExerciseType> _exerciseTypes;

        public IList<IExerciseType> ExerciseTypes
        {
            get { return (from e in _exerciseTypes orderby e.DisplayOrder descending, e.Id select e).ToArray(); }
            set
            {
                _exerciseTypes = value;
                Dispatcher("ExerciseTypes");
            }
        }

        private readonly IRepository _repository;


        public HomeViewModel(ILog log, IAccount account, ILocalize localize, IApplication application, IHistory history,
                             INavigationService navigationService, IUser user, IRepository repository,
                             IList<IExerciseType> exerciseTypes)
        {
            _log = log;
            _localize = localize;
            _application = application;
            Account = account;

            _history = history;
            _history.OnHistoryItemsChanged += _history_OnHistoryItemsChanged;
            _NavigationService = navigationService;
            User = user;
            ExerciseTypes = exerciseTypes;
            _repository = repository;

            _history.Load();

            _repository.Single<User>(1).ContinueWith(t =>
                {
                    var foundUser = t.Result;
                    if (foundUser == null)
                    {
                        //this is first load of the app, set it up
                        _repository.Insert<User>(this.User).ContinueWith(task =>
                            {
                                this.User = this.User;
                            });
                    }
                    else
                    {
                        User = foundUser;
                    }
                    Account.AccessToken = user.RunkeeperToken;
                });

            if (_exerciseTypes == null || _exerciseTypes.Count == 0 ||
                (_exerciseTypes.Count == 1 && _exerciseTypes[0].Id == 0))
            {
                if (HomeViewModel.cachedTypes != null)
                {
                    this.ExerciseTypes = HomeViewModel.cachedTypes;
                    _log.Info("cache hit");
                }
                else
                {
                    _log.Info("cache miss");
                    this.ExerciseTypes = DefaultTypes;
                    _log.Info("default types set, querying");
                    _repository.Query<ExerciseType>("select * from ExerciseType").ContinueWith(t =>
                        {
                            _log.Info("query complete");
                            var types = t.Result;
                            if (types == null || types.Count == 0)
                            {

                                _log.Info("db does not have Exercise types, loading default items");
                                foreach (var e in from tt in this.ExerciseTypes orderby tt.Id select tt)
                                {
                                    _repository.Insert<ExerciseType>(e);
                                }
                            }
                            else
                            {
                                _log.Info("all excecise types retreived from the db, update local data store");
                                this.ExerciseTypes = (from tt in types select tt).ToArray();
                            }
                            _log.Info("cache extypes to static var");
                            HomeViewModel.cachedTypes = ExerciseTypes;

                        });
                }
            }
        }

        void _history_OnHistoryItemsChanged(IHistory history, IList<IHistoryItem> changedItems)
        {
            Dispatcher("History");
        }

        private static IList<IExerciseType> cachedTypes = null;

        public string AppTitle
        {
            get { return _application.Name; }
            set
            {
                _application.Name = value;
                Dispatcher("AppTitle");
            }
        }


        public IHistory History
        {
            get { return _history; }
            set
            {
                _history = value;
                Dispatcher("History");
            }
        }


        private ICommand _pairCommand;

        public ICommand PairAgentCommand
        {
            get
            {
                return _pairCommand
                       ?? (_pairCommand = new DelegateCommand(t =>
                           {
                               PairDevice();
                           }
                                              ));

            }
        }

        private ICommand _LoginWithRunKeeperCommand;

        public ICommand LoginWithRunKeeperCommand
        {
            get
            {
                return _LoginWithRunKeeperCommand
                       ?? (_LoginWithRunKeeperCommand = new DelegateCommand(t =>
                           {
                               _NavigationService.Navigate(new Uri("/UI/OAuthWebViewPage.xaml",
                                                                   UriKind.RelativeOrAbsolute));
                           }
                                                            ));

            }
        }

        private void PairDevice()
        {
            MessageBox.Show("Pair Device with AGENT");
        }



        private ICommand _exerciseCommand;

        public ICommand ExerciseCommand
        {
            get
            {
                return _exerciseCommand
                       ?? (_exerciseCommand = new DelegateCommand(t =>
                           {
                               var button =
                                   ((System.Windows.Controls.Primitives.ButtonBase)
                                    (((System.Windows.RoutedEventArgs) (t)).OriginalSource));
                               _NavigationService.Navigate(new Uri("/MainPage.xaml?type=" + button.CommandParameter,
                                                                   UriKind.RelativeOrAbsolute));
                           }
                                                  ));

            }
        }

        public IList<IExerciseType> DefaultTypes
        {
            get
            {
                return new List<IExerciseType>()
                    {

                        new ExerciseType()
                            {
                                Id = 1,
                                DisplayName = "Walking",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName = "Walking",
                                Icon = "/Assets/Exercises/walking.png"
                            },
                        new ExerciseType()
                            {
                                Id = 2,
                                DisplayName = "Running",
                                DisplayOrder = 9,
                                UIExerciseType = "Map",
                                TypeName = "Running",
                                Icon = "/Assets/Exercises/walking.png"
                            },
                        new ExerciseType()
                            {
                                Id = 3,
                                DisplayName = "Mountain Biking",
                                DisplayOrder = 8,
                                UIExerciseType = "Map",
                                TypeName = "MountainBiking",
                                Icon = "/Assets/Exercises/mountainbike.png"
                            },
                        new ExerciseType()
                            {
                                Id = 4,
                                DisplayName = "Cycling",
                                DisplayOrder = 7,
                                UIExerciseType = "Map",
                                TypeName = "Cycling",
                                Icon = "/Assets/Exercises/bike.png"
                            },
                        new ExerciseType()
                            {
                                Id = 5,
                                DisplayName = "Hiking",
                                DisplayOrder = 6,
                                UIExerciseType = "Map",
                                TypeName = "Hiking",
                                Icon = "/Assets/Exercises/hike.png"
                            },
                        new ExerciseType()
                            {
                                Id = 6,
                                DisplayName = "Downhill Skiing",
                                DisplayOrder = 5,
                                UIExerciseType = "Map",
                                TypeName = "DownhillSkiing",
                                Icon = "/Assets/Exercises/skiing.png"
                            },
                        new ExerciseType()
                            {
                                Id = 7,
                                DisplayName = "Cross Country Skiing",
                                DisplayOrder = 4,
                                UIExerciseType = "Map",
                                TypeName = "CrossCountrySkiing",
                                Icon = "/Assets/Exercises/crosscountryskiing.png"
                            },
                        new ExerciseType()
                            {
                                Id = 8,
                                DisplayName = "Snowboarding",
                                DisplayOrder = 3,
                                UIExerciseType = "Map",
                                TypeName = "Snowboarding",
                                Icon = "/Assets/Exercises/snowboarding.png"
                            },
                        new ExerciseType()
                            {
                                Id = 9,
                                DisplayName = "Wheelchair",
                                DisplayOrder = 2,
                                UIExerciseType = "Map",
                                TypeName = "Wheelchair",
                                Icon = "/Assets/Exercises/wheelchair.png"
                            },
                        new ExerciseType()
                            {
                                Id = 10,
                                DisplayName = "Other",
                                DisplayOrder = 1,
                                UIExerciseType = "Map",
                                TypeName = "Other",
                                Icon = "/Assets/Exercises/other.png"
                            },
                    };


            }
        }
    }
}