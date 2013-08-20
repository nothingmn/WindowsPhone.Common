using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Exercisr.Contracts.Configuration;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Exercisr.Contracts.Exercise;
using WindowsPhone.Contracts.Membership;

namespace Exercisr.Contracts.ViewModels
{
    public interface IHomeViewModel
    {
        string AppTitle { get; }
        IHistory History { get; set; }
        IUser User { get; set; }
        IList<IExerciseType> ExerciseTypes { get; set; }
        ICommand ExerciseCommand { get; }
        ICommand PairAgentCommand { get; }
        ICommand LoginWithRunKeeperCommand { get; }

        ISettings Settings { get; set; }
        ICommand MetricToggleCommand { get; }
        ICommand AutoPostToRunKeeperCommand { get; }
        ICommand PostToRunKeeperCommand { get; }
        
    }
}
