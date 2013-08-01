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
using RunKeeper8.Contracts.Exercise;
using RunKeeper8.Contracts.Geo;
using RunKeeper8.Contracts.Services;
using RunKeeper8.Contracts.ViewModels;
using RunKeeper8.Domain.Exercise;
using RunKeeper8.Domain.Geo;
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
            get { return (from e in _exerciseTypes orderby e.DisplayOrder select e).ToArray(); }
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
            _NavigationService = navigationService;
            User = user;
            ExerciseTypes = exerciseTypes;
            _repository = repository;

            _repository.Single<User>(1).ContinueWith(t =>
                {
                    var foundUser = t.Result;
                    if (foundUser == null)
                    {
                        //this is first load of the app, set it up
                        var u = new User();
                        _repository.Insert<User>(user).ContinueWith(task =>
                            {
                                u.Id = task.Result;
                                this.User = u;
                            });
                    }
                    else
                    {
                        User = foundUser;
                    }
                    Account.AccessToken = user.RunkeeperToken;
                });

            if (this.ExerciseTypes == null || this.ExerciseTypes.Count == 0 || (this.ExerciseTypes.Count == 1 && this.ExerciseTypes[0].Id == 0))
            {
                _repository.Query<ExerciseType>("select * from ExerciseType").ContinueWith(t =>
                    {
                        var types = t.Result;
                        if (types == null || types.Count == 0)
                        {
                            foreach (var e in this.DefaultTypes)
                            {

                                _repository.Insert<ExerciseType>(e);
                            }
                            this.ExerciseTypes = DefaultTypes;
                        }
                        else
                        {
                            this.ExerciseTypes = (from tt in types select tt).ToArray();
                        }
                    });
            }
        }


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
                                DisplayOrder = 0,
                                UIExerciseType = "Map",
                                TypeName="Walking",
                                Icon="/Assets/Exercises/walking.png"
                            },
                        new ExerciseType()
                            {
                                Id = 2,
                                DisplayName = "Running",
                                DisplayOrder = 1,
                                UIExerciseType = "Map",
                                TypeName="Running",
                                Icon="/Assets/Exercises/walking.png"
                            },
                        new ExerciseType()
                            {
                                Id = 3,
                                DisplayName = "Mountain Biking",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName="MountainBiking",
                                Icon="/Assets/Exercises/mountainbike.png"
                            },
                        new ExerciseType()
                            {
                                Id = 4,
                                DisplayName = "Cycling",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName="Cycling",
                                Icon="/Assets/Exercises/bike.png"
                            },
                        new ExerciseType()
                            {
                                Id = 5,
                                DisplayName = "Hiking",
                                DisplayOrder = 2,
                                UIExerciseType = "Map",
                                TypeName="Hiking",
                                Icon="/Assets/Exercises/hike.png"
                            },
                        new ExerciseType()
                            {
                                Id = 6,
                                DisplayName = "Downhill Skiing",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName="DownhillSkiing",
                                Icon="/Assets/Exercises/skiing.png"
                            },
                        new ExerciseType()
                            {
                                Id = 7,
                                DisplayName = "Cross Country Skiing",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName="CrossCountrySkiing",
                                Icon="/Assets/Exercises/crosscountryskiing.png"
                            },
                        new ExerciseType()
                            {
                                Id = 8,
                                DisplayName = "Snowboarding",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName="Snowboarding",
                                Icon="/Assets/Exercises/snowboarding.png"
                            },
                        new ExerciseType()
                            {
                                Id = 9,
                                DisplayName = "Wheelchair",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName="Wheelchair",
                                Icon="/Assets/Exercises/wheelchair.png"
                            },
                        new ExerciseType()
                            {
                                Id = 10,
                                DisplayName = "Other",
                                DisplayOrder = 10,
                                UIExerciseType = "Map",
                                TypeName="Other",
                                Icon="/Assets/Exercises/other.png"
                            },
                    };


            }
        }
    }
}