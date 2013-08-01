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
using RunKeeper8.Contracts.Exercise;
using WindowsPhone.Contracts.Membership;

namespace RunKeeper8.Contracts.ViewModels
{
    public interface IHomeViewModel
    {
        string AppTitle { get; }
        IHistory History { get; set; }
        IUser User { get; set; }
        IList<IExerciseType> ExerciseTypes { get; set; }
        ICommand BookmarkCommand { get; }
        ICommand ExerciseCommand { get; }
        
        
    }
}
