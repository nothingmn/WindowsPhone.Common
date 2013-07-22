using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
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
using RunKeeper8.Contracts.Geo;
using RunKeeper8.Contracts.Services;
using RunKeeper8.Contracts.ViewModels;
using RunKeeper8.Domain.Geo;
using WindowsPhone.Common.Command;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Localization;
using WindowsPhone.Contracts.Logging;
using System.ComponentModel;

namespace RunKeeper8.Domain.ViewModels
{
    public class TrackingViewModel : ViewModelBase, ITrackingViewModel, INotifyPropertyChanged
    {


        private ILog _log;
        private ILocalize _localize;
        private IApplication _application;
        private IGeoPositionWatcher<GeoCoordinate> _coordinateProvider;

        private DispatcherTimer _timer = new DispatcherTimer();
        private long _startTime;
        private DateTime _startedAt;

        private bool _started = false;
        //ID_CAP_LOCATION
        private double _kilometres;
        private long _previousPositionChangeTick;

        public IAccount Account { get; set; }

        public TrackingViewModel(ILog log, IAccount account, ILocalize localize, IApplication application,
                                 IGeoPositionWatcher<GeoCoordinate> coordinateProvider)
        {
            _log = log;
            _localize = localize;
            _application = application;
            _coordinateProvider = coordinateProvider;
            Account = account;

            _started = false;
            _startTime = System.Environment.TickCount;

            _coordinateProvider.PositionChanged += _coordinateProvider_PositionChanged;
            _coordinateProvider.Start();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            //MapCenter = new GeoCoordinate(coordinateProvider.Position.Location.Latitude, coordinateProvider.Position.Location.Longitude);
            MapCenter = new GeoCoordinate(0,0);
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
            Coordinates = new GeoCoordinateCollection();
        }

        private void _coordinateProvider_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {

            var coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude, e.Position.Location.Altitude, e.Position.Location.HorizontalAccuracy, e.Position.Location.VerticalAccuracy, e.Position.Location.Speed, e.Position.Location.Course);
            if (_started)
            {

                if (StrokeThickness <= 0) StrokeThickness = 1;
                GeoCoordinate prev = MapCenter;
                if (Coordinates.Count > 0) prev = Coordinates.Last();
                var previousPoint = prev;
                var distance = coord.GetDistanceTo(previousPoint);
                if (distance > 0)
                {
                    _log.InfoFormat("Distance:{0}", distance);
                    var millisPerKilometer = (1000.0/distance)*(System.Environment.TickCount - _previousPositionChangeTick);

                    _kilometres += distance/1000.0;
                    if (ZoomLevel < 18) ZoomLevel = 18;

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

                    this.Coordinates.Add(coord);
                    OnPropertyChanged("Coordinates");
                    _previousPositionChangeTick = System.Environment.TickCount;


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

        public ICommand PairCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new DelegateCommand(PairDevice);
                }
                return searchCommand;
            }
        }

        private void PairDevice()
        {

        }

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

        public void Start()
        {
            if (_started)
            {
                _timer.Stop();
                _coordinateProvider.Stop();
                _started = false;


                
                if (!string.IsNullOrEmpty(Account.AccessToken))
                {
                    var result = MessageBox.Show("Do you want to publish this to RunKeeper?", "Publish?", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        var activity = WindowsPhone.DI.Container.Current.Get<IActivity>();
                        activity.equipment = "None";
                        activity.type = "Walking";
                        if (Pace.TotalMinutes > 120) activity.type = "Running";
                        activity.start_time = _startedAt.ToLongTimeString();
                        
                        
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
                            if (counter == 0) path.type = "Start";
                            if (counter == Coordinates.Count-1) path.type = "End";
                            activity.path.Add(path);
                            counter++;
                        }
                        


                        var publisher = WindowsPhone.DI.Container.Current.Get<IPublishActivity>();
                        publisher.Publish(activity);
                    }
                }

            }
            else
            {
                _timer.Start();
                _coordinateProvider.Start();
                _started = true;
                _startedAt = DateTime.Now;

                
            }
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

        }


        private GeoCoordinate _MapCenter;
        public GeoCoordinate MapCenter
        {
            get { return _MapCenter; }
            set
            {
                _MapCenter = value;
                base.OnPropertyChanged("MapCenter");
            }
        }



        private System.Windows.Media.Color _StrokeColor;
        public System.Windows.Media.Color StrokeColor { get { return _StrokeColor; } set { _StrokeColor = value; OnPropertyChanged("StrokeColor"); } }

        private double _StrokeThickness;
        public double StrokeThickness { get { return _StrokeThickness; } set { _StrokeThickness = value; OnPropertyChanged("StrokeThickness"); } }

        
        private GeoCoordinateCollection _Coordinates;
        public GeoCoordinateCollection Coordinates { get { return _Coordinates; } set { _Coordinates = value; OnPropertyChanged("Coordinates"); } }



        private double _ZoomLevel;
        public double ZoomLevel { get { return _ZoomLevel; } set { _ZoomLevel = value; OnPropertyChanged("ZoomLevel"); } }
        private double _Heading;
        public double Heading { get { return _Heading; } set { _Heading = value; OnPropertyChanged("Heading"); } }
        private double _Pitch;
        public double Pitch { get { return _Pitch; } set { _Pitch = value; OnPropertyChanged("Pitch"); } }
        private bool _PedestrianFeaturesEnabled;
        public bool PedestrianFeaturesEnabled { get { return _PedestrianFeaturesEnabled; } set { _PedestrianFeaturesEnabled = value; OnPropertyChanged("PedestrianFeaturesEnabled"); } }
        private bool _LandmarksEnabled;
        public bool LandmarksEnabled { get { return _LandmarksEnabled; } set { _LandmarksEnabled = value; OnPropertyChanged("LandmarksEnabled"); } }

        private double _Distance;
        public double Distance { get { return _Distance; } set { _Distance = value; OnPropertyChanged("Distance"); } }
        private TimeSpan _Time;
        public TimeSpan Time { get { return _Time; } set { _Time = value; OnPropertyChanged("Time"); } }
        private double _Calories;
        public double Calories { get { return _Calories; } set { _Calories = value; OnPropertyChanged("Calories"); } }
        private TimeSpan _Pace;
        public TimeSpan Pace { get { return _Pace; } set { _Pace = value; OnPropertyChanged("Pace"); } }

        private string _DistanceDisplay;
        public string DistanceDisplay { get { return _DistanceDisplay; } set { _DistanceDisplay = value; OnPropertyChanged("DistanceDisplay"); } }

        private string _TimeDisplay;
        public string TimeDisplay { get { return _TimeDisplay; } set { _TimeDisplay = value; OnPropertyChanged("TimeDisplay"); } }
        private string _CaloriesDisplay;
        public string CaloriesDisplay { get { return _CaloriesDisplay; } set { _CaloriesDisplay = value; OnPropertyChanged("CaloriesDisplay"); } }
        private string _PaceDisplay;
        public string PaceDisplay { get { return _PaceDisplay; } set { _PaceDisplay = value; OnPropertyChanged("PaceDisplay"); } }




    }
}