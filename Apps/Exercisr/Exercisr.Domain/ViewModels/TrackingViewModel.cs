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
using System.Windows.Threading;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using Exercisr.Contracts.Exercise;
using Exercisr.Contracts.Geo;
using Exercisr.Contracts.Services;
using Exercisr.Contracts.ViewModels;
using Exercisr.Domain.Exercise;
using Exercisr.Domain.Geo;
using WindowsPhone.Common.Command;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Localization;
using WindowsPhone.Contracts.Logging;
using System.ComponentModel;
using WindowsPhone.Contracts.Repository;

namespace Exercisr.Domain.ViewModels
{
    public class TrackingViewModel : ViewModelBase, ITrackingViewModel, INotifyPropertyChanged
    {
        private IExerciseType _exerciseType;

        public IExerciseType ExerciseType
        {
            get { return _exerciseType; }
            set
            {
                _exerciseType = value;
                Dispatcher("ExerciseType");
            }
        }

        public void SetExerciseTypeId(int Id)
        {

            //_repository.Query<ExerciseType>("select * from ExerciseType where Id=?", Id).ContinueWith(t =>

            _repository.Single<ExerciseType>(Id).ContinueWith(t =>
                {
                    //if (t.Result != null && t.Result.Count == 1)
                    //{
                    //ExerciseType = t.Result[0];
                    ExerciseType = t.Result;
                    ExerciseType.DisplayOrder++;
                    _repository.Update<ExerciseType>(ExerciseType, ExerciseType.Id);
                    //}
                });
        }


        private ILog _log;
        private ILocalize _localize;
        private IApplication _application;
        private IGeoPositionWatcher<GeoCoordinate> _coordinateProvider;

        private DispatcherTimer _timer = new DispatcherTimer();
        private long _startTime;
        private DateTime _startedAt;

        private bool _paused = false;

        private bool _started = false;
        //ID_CAP_LOCATION
        private double _kilometres;
        private long _previousPositionChangeTick;

        public IAccount Account { get; set; }

        private IHistory _history;
        private readonly IRepository _repository;

        public TrackingViewModel(ILog log, IAccount account, ILocalize localize, IApplication application, IGeoPositionWatcher<GeoCoordinate> coordinateProvider, IHistory history, IRepository repository)
        {
            _log = log;
            _localize = localize;
            _application = application;
            _coordinateProvider = coordinateProvider;
            _history = history;
            _repository = repository;
            Account = account;        

            _started = false;
            _startTime = System.Environment.TickCount;

            _coordinateProvider.PositionChanged += _coordinateProvider_PositionChanged;
            _coordinateProvider.Start();
            UICoordinates = new GeoCoordinateCollection();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            //MapCenter = new GeoCoordinate(coordinateProvider.Position.Location.Latitude, coordinateProvider.Position.Location.Longitude);
            MapCenter = new GeoCoordinate(0, 0);
            Heading = 0;
            ZoomLevel = 15;
            Pitch = 55;
            PedestrianFeaturesEnabled = true;
            LandmarksEnabled = true;

            DistanceDisplay = "0 km";
            PaceDisplay = "00:00";
            CaloriesDisplay = "0";
            TimeDisplay = "00:00";

            StrokeColor = System.Windows.Media.Colors.Red;
            StrokeThickness = 5;
            Coordinates.Clear();

            StartVisibility = (!_started ? Visibility.Visible : Visibility.Collapsed);
            StopVisibility = (_started ? Visibility.Visible : Visibility.Collapsed);
            PauseVisibility = (!_paused ? Visibility.Visible : Visibility.Collapsed);
            ResumeVisibility = (_paused ? Visibility.Visible : Visibility.Collapsed);

        }


        private void _coordinateProvider_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {

            var coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude,
                                          e.Position.Location.Altitude, e.Position.Location.HorizontalAccuracy,
                                          e.Position.Location.VerticalAccuracy, e.Position.Location.Speed,
                                          e.Position.Location.Course);
            if (_started && !_paused)
            {

                if (StrokeThickness <= 0) StrokeThickness = 1;
                GeoCoordinate prev = MapCenter;
                if (Coordinates.Count > 0) prev = (GeoCoordinate)Coordinates.Last();
                var previousPoint = prev;
                var distance = coord.GetDistanceTo(previousPoint);
                if (distance > 0)
                {
                    _log.InfoFormat("Distance:{0}", distance);
                    double millisPerKilometer = (1000.0/distance)* (System.Environment.TickCount - _previousPositionChangeTick);

                    _kilometres += distance/1000.0;
                    if (ZoomLevel < 18) ZoomLevel = 18;

                    if (millisPerKilometer > int.MaxValue) millisPerKilometer = int.MaxValue;
                    Pace = TimeSpan.FromMilliseconds(millisPerKilometer);
                    Calories = _kilometres*65;
                    Distance = _kilometres;

                    PaceDisplay = Pace.ToString(@"mm\:ss");
                    CaloriesDisplay = string.Format("{0:f0}", Calories);
                    DistanceDisplay = string.Format("{0:f2} km", Distance);

                    PositionHandler handler = new PositionHandler();
                    Heading = handler.CalculateBearing(new Position(previousPoint), new Position(coord));


                    ShellTile.ActiveTiles.First().Update(new IconicTileData()
                        {

                            Title = _application.ToString(),
                            WideContent1 = DistanceDisplay,
                            WideContent2 = CaloriesDisplay,
                        });

                    
                    this.Coordinates.Add(new Coordinate()
                        {
                            Altitude = coord.Altitude,
                            Course = coord.Course,
                            HorizontalAccuracy = coord.HorizontalAccuracy,
                            Latitude = coord.Latitude,
                            Longitude = coord.Longitude,
                            Speed = coord.Speed,
                            VerticalAccuracy = coord.VerticalAccuracy
                        });

                    UICoordinates.Add(coord);
                    Dispatcher("Coordinates");
                    _previousPositionChangeTick = System.Environment.TickCount;


                    MapCenter = coord;

                }
            }
            else
            {
                MapCenter = coord;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
            TimeDisplay = Time.ToString(@"hh\:mm\:ss");
        }

        private ICommand runKeeperLoginCommand;

        public ICommand RunKeeperLoginCommand
        {
            get
            {
                if (runKeeperLoginCommand == null)
                {
                    runKeeperLoginCommand = new DelegateCommand(LoginUser);
                }
                return runKeeperLoginCommand;
            }
        }

        private void LoginUser()
        {

        }

        private ICommand searchCommand;


        private ICommand startCommand;

        public ICommand StartCommand
        {
            get
            {
                if (startCommand == null)
                {
                    startCommand = new DelegateCommand(Start);
                }
                return startCommand;
            }
        }
        private ICommand stopCommand;

        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                {
                    stopCommand = new DelegateCommand(Stop);
                }
                return stopCommand;
            }
        }

        public void Stop()
        {

            _timer.Stop();
            _coordinateProvider.Stop();
            _started = false;
            Heading = 0;
            ZoomLevel = 15;
            Pitch = 55;

         
            var activity = WindowsPhone.DI.Container.Current.Get<IActivity>();

            activity.detect_pauses = true;
            activity.duration = Time.TotalSeconds;
            activity.total_calories = Calories;
            activity.total_distance = Distance/1000; //distance is in KM's and we need M's
            activity.equipment = "None";
            activity.type = ExerciseType.TypeName;
            activity.start_time = _startedAt.ToUniversalTime().ToString(DateTimeFormatInfo.CurrentInfo.RFC1123Pattern);


            activity.notes = string.Format("I just {0} for ", activity.type.Replace("ing", "ed"));
            if (Time.TotalHours >= 1)
            {
                activity.notes = activity.notes +
                                 string.Format("{0} hours, ", Time.TotalHours.ToString("0"));
            }
            activity.notes = activity.notes +
                             string.Format(
                                 "{0} minutes for a distance of {2}, and burned {1} calories!",
                                 Time.TotalMinutes.ToString("0.0"),
                                 activity.total_calories.ToString("0.0"),
                                 activity.total_distance.ToString("0.0"));

            //move to settings
            activity.post_to_facebook = false;
            activity.post_to_twitter = false;

            activity.path = new List<IPath>();

            var counter = 0;
            foreach (var c in Coordinates)
            {
                var path = WindowsPhone.DI.Container.Current.Get<IPath>();
                path.altitude = c.Altitude;
                path.latitude = c.Latitude;
                path.longitude = c.Longitude;
                if (counter == 0)
                {
                    path.type = "Start";
                }
                else if (counter == Coordinates.Count - 1)
                {
                    path.type = "End";
                }
                else
                {
                    path.type = "gps";
                }

                activity.path.Add(path);
                counter++;
            }

            IHistoryItem item = WindowsPhone.DI.Container.Current.Get<IHistoryItem>();
            item.DisplayName = string.Format("{0} - {1}", ExerciseType.DisplayName, DateTime.Now.ToString(DateTimeFormatInfo.CurrentInfo.RFC1123Pattern));
            item.StartTimestamp = _startedAt;
            item.ExerciseType = this.ExerciseType.ToString();
            item.Coordinates = this.Coordinates;
            item.Calories = this.Calories;
            item.Distance = this.Distance;
            item.EndTimestamp = DateTime.Now;
            item.Pace = this.Pace;

            _history.HistoryItems.Add(item);
            _history.Save();

            if (!string.IsNullOrEmpty(Account.AccessToken))
            {
                var result = MessageBox.Show("Do you want to publish this to RunKeeper?", "Publish?",
                                             MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    var publisher = WindowsPhone.DI.Container.Current.Get<IPublishActivity>();
                    publisher.OnPublishComplete += publisher_OnPublishComplete;
                    publisher.Publish(activity);

                }
            }
            StartVisibility = (!_started ? Visibility.Visible : Visibility.Collapsed);
            StopVisibility = (_started ? Visibility.Visible : Visibility.Collapsed);
            _paused = false;


            ShellTile.ActiveTiles.First().Update(new IconicTileData()
            {

                Title = _application.ToString(),
                WideContent1 = "",
                WideContent2 = "",
            });

        }

        public void Start()
        {
            Time = TimeSpan.FromMilliseconds(0);
            Calories = 0;
            Distance = 0;
            Pace = TimeSpan.FromMilliseconds(0);
            Coordinates.Clear();
            UICoordinates.Clear();
            _startTime = System.Environment.TickCount;
            _startedAt = DateTime.Now;

            DistanceDisplay = "0 km";
            PaceDisplay = "00:00";
            CaloriesDisplay = "0";
            TimeDisplay = "00:00";

            _startedAt = DateTime.Now;
            _paused = false;

            _timer.Start();
            _coordinateProvider.Start();
            _started = true;

            StartVisibility = (!_started ? Visibility.Visible : Visibility.Collapsed);
            StopVisibility = (_started ? Visibility.Visible : Visibility.Collapsed);
            PauseVisibility = (!_paused ? Visibility.Visible : Visibility.Collapsed);
            ResumeVisibility = (_paused ? Visibility.Visible : Visibility.Collapsed);

            Dispatcher("Coordinates");
        }


        public DateTime PublishDateTime { get; set; }
        private bool _PublishSuccess = false;

        public bool PublishSuccess
        {
            get { return _PublishSuccess; }
            set
            {
                _PublishSuccess = value;
                Dispatcher("PublishSuccess");
            }
        }

        public string PublishBody { get; set; }

        private void publisher_OnPublishComplete(IPublishActivity publishActivity, DateTime timestamp, bool success,
                                                 Exception e, string body)
        {
            PublishDateTime = timestamp;
            PublishSuccess = success;
            PublishBody = body;

        }


        private ICommand connectCommand;

        public ICommand ConnectCommand
        {
            get
            {
                if (connectCommand == null)
                {
                    connectCommand = new DelegateCommand(Start);
                }
                return connectCommand;
            }
        }

        public void Connect()
        {

        }


        private ICommand resumeCommand;

        public ICommand ResumeCommand
        {
            get
            {
                if (resumeCommand == null)
                {
                    resumeCommand = new DelegateCommand(Resume);
                }
                return resumeCommand;
            }
        }

        public void Resume()
        {
            _paused = !_paused;

            PauseVisibility = (!_paused ? Visibility.Visible : Visibility.Collapsed);
            ResumeVisibility = (_paused ? Visibility.Visible : Visibility.Collapsed);

            if(_paused) 
                _timer.Stop();
            else 
                _timer.Start();
        }


        private GeoCoordinate _MapCenter;

        public GeoCoordinate MapCenter
        {
            get { return _MapCenter; }
            set
            {
                _MapCenter = value;
                base.Dispatcher("MapCenter");
            }
        }



        private System.Windows.Media.Color _StrokeColor;

        public System.Windows.Media.Color StrokeColor
        {
            get { return _StrokeColor; }
            set
            {
                _StrokeColor = value;
                Dispatcher("StrokeColor");
            }
        }

        private double _StrokeThickness;

        public double StrokeThickness
        {
            get { return _StrokeThickness; }
            set
            {
                _StrokeThickness = value;
                Dispatcher("StrokeThickness");
            }
        }


        private IList<ICoordinate> _Coordinates = new List<ICoordinate>();

        public IList<ICoordinate> Coordinates
        {
            get { return _Coordinates; }
            set
            {
                _Coordinates = value;
                Dispatcher("Coordinates");
            }
        }



        private double _ZoomLevel;

        public double ZoomLevel
        {
            get { return _ZoomLevel; }
            set
            {
                _ZoomLevel = value;
                Dispatcher("ZoomLevel");
            }
        }

        private double _Heading;

        public double Heading
        {
            get { return _Heading; }
            set
            {
                _Heading = value;
                Dispatcher("Heading");
            }
        }

        private double _Pitch;

        public double Pitch
        {
            get { return _Pitch; }
            set
            {
                _Pitch = value;
                Dispatcher("Pitch");
            }
        }

        private bool _PedestrianFeaturesEnabled;

        public bool PedestrianFeaturesEnabled
        {
            get { return _PedestrianFeaturesEnabled; }
            set
            {
                _PedestrianFeaturesEnabled = value;
                Dispatcher("PedestrianFeaturesEnabled");
            }
        }

        private bool _LandmarksEnabled;

        public bool LandmarksEnabled
        {
            get { return _LandmarksEnabled; }
            set
            {
                _LandmarksEnabled = value;
                Dispatcher("LandmarksEnabled");
            }
        }

        private double _Distance;

        public double Distance
        {
            get { return _Distance; }
            set
            {
                _Distance = value;
                Dispatcher("Distance");
            }
        }

        private TimeSpan _Time;

        public TimeSpan Time
        {
            get { return _Time; }
            set
            {
                _Time = value;
                Dispatcher("Time");
            }
        }

        private double _Calories;

        public double Calories
        {
            get { return _Calories; }
            set
            {
                _Calories = value;
                Dispatcher("Calories");
            }
        }

        private TimeSpan _Pace;

        public TimeSpan Pace
        {
            get { return _Pace; }
            set
            {
                _Pace = value;
                Dispatcher("Pace");
            }
        }

        private string _DistanceDisplay;

        public string DistanceDisplay
        {
            get { return _DistanceDisplay; }
            set
            {
                _DistanceDisplay = value;
                Dispatcher("DistanceDisplay");
            }
        }

        private string _TimeDisplay;

        public string TimeDisplay
        {
            get { return _TimeDisplay; }
            set
            {
                _TimeDisplay = value;
                Dispatcher("TimeDisplay");
            }
        }

        private string _CaloriesDisplay;

        public string CaloriesDisplay
        {
            get { return _CaloriesDisplay; }
            set
            {
                _CaloriesDisplay = value;
                Dispatcher("CaloriesDisplay");
            }
        }

        private string _PaceDisplay;

        public string PaceDisplay
        {
            get { return _PaceDisplay; }
            set
            {
                _PaceDisplay = value;
                Dispatcher("PaceDisplay");
            }
        }

        private Visibility _StopVisibility;
        public Visibility StopVisibility
        {
            get { return _StopVisibility; }
            set
            {
                _StopVisibility = value;
                Dispatcher("StopVisibility");
            }
        }
        private Visibility _StartVisibility;
        public Visibility StartVisibility
        {
            get { return _StartVisibility; }
            set
            {
                _StartVisibility = value;
                Dispatcher("StartVisibility");
            }
        }
        private Visibility _PauseVisibility;
        public Visibility PauseVisibility
        {
            get { return _PauseVisibility; }
            set
            {
                _PauseVisibility = value;
                Dispatcher("PauseVisibility");
            }
        }

        public GeoCoordinateCollection UICoordinates { get; set; }
        private Visibility _ResumeVisibility;
        public Visibility ResumeVisibility
        {
            get { return _ResumeVisibility; }
            set
            {
                _ResumeVisibility = value;
                Dispatcher("ResumeVisibility");
            }
        }



    }
}