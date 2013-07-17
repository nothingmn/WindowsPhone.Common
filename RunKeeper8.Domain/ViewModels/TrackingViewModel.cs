using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.ViewModels;
using WindowsPhone.Common.ViewModels;

namespace RunKeeper8.Domain.ViewModels
{
    public class TrackingViewModel : ViewModelBase, ITrackingViewModel 
    {
        public double Distance { get; set; }
        public double Time { get; set; }
        public double Calories { get; set; }
        public double Pace { get; set; }

        public string StartButtonContent { get; set; }
        public string ResumeButtonContent { get; set; }
    }
}
