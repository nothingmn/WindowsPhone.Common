using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Exercisr.Contracts.Geo;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Exercisr.Contracts.Exercise;

namespace Exercisr.Contracts.ViewModels
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

        IExerciseType ExerciseType { get; set; }
        void SetExerciseTypeId(int Id);
        
        double ZoomLevel { get; set; }
        double Heading { get; set; }
        double Pitch { get; set; }

        bool PedestrianFeaturesEnabled { get; set; }
        bool LandmarksEnabled { get; set; }

        System.Windows.Media.Color StrokeColor { get; set; }
        double StrokeThickness { get; set; }
        IList<ICoordinate> Coordinates { get; set; }

        GeoCoordinateCollection UICoordinates { get; set; }


        //commands
        ICommand StartCommand { get; }
        ICommand ConnectCommand { get; }
        ICommand ResumeCommand { get; }


        DateTime PublishDateTime { get; set; }
        bool PublishSuccess { get; set; }
        string PublishBody { get; set; }


        Visibility StopVisibility { get; set; }
        Visibility StartVisibility { get; set; }
        Visibility ResumeVisibility { get; set; }
        Visibility PauseVisibility { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

    }
}