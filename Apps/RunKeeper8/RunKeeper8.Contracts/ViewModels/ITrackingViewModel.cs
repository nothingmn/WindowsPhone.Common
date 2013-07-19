using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;

namespace RunKeeper8.Contracts.ViewModels
{
    public interface ITrackingViewModel
    {
        //storage values
        double Distance { get; set; }
        TimeSpan Time { get; set; }
        double Calories { get; set; }
        TimeSpan Pace { get; set; }

        string DistanceDisplay { get; set; }
        string TimeDisplay { get; set; }
        string CaloriesDisplay { get; set; }
        string PaceDisplay { get; set; }

        GeoCoordinate MapCenter { get; set; }

        double ZoomLevel { get; set; }
        double Heading { get; set; }
        double Pitch { get; set; }

        bool PedestrianFeaturesEnabled { get; set; }
        bool LandmarksEnabled { get; set; }

        System.Windows.Media.Color StrokeColor { get; set; }
        double StrokeThickness { get; set; }
        GeoCoordinateCollection Coordinates { get; set; }


        //commands
        ICommand PairCommand { get; }
        ICommand StartCommand { get; }
        ICommand ConnectCommand { get; }
        ICommand ResumeCommand { get; }

        ICommand RunKeeperLoginCommand { get; }

    }
}