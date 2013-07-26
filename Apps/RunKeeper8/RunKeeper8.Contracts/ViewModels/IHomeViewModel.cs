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
using RunKeeper8.Contracts.Excercise;

namespace RunKeeper8.Contracts.ViewModels
{
    public interface IHomeViewModel
    {
        string AppTitle { get; }
        IHistory History { get; set; }


        ICommand BookmarkCommand { get; }
        ICommand RunningCommand { get; }
        ICommand WalkingCommand { get; }
        ICommand CyclingCommand { get; }
        ICommand MountainBikingCommand { get; }
        ICommand HikingCommand { get; }
        ICommand DownhillSkiingCommand { get; }
        ICommand CrossCountrySkiingCommand { get; }
        ICommand SnowboardingCommand { get; }
        ICommand WheelchairCommand { get; }
        ICommand OtherCommand { get; }
        
        
    }
}