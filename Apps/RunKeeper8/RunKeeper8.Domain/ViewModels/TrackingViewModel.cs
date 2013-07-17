using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.ViewModels;
using WindowsPhone.Common.ViewModels;
using WindowsPhone.Contracts.Logging;

namespace RunKeeper8.Domain.ViewModels
{
    public class TrackingViewModel : ViewModelBase, ITrackingViewModel
    {

        private ILog _log;
        public TrackingViewModel(ILog log)
        {
            _log = log;
        }
        public double Distance { get; set; }
        public double Time { get; set; }
        public double Calories { get; set; }
        public double Pace { get; set; }

        public string StartButtonContent { get; set; }
        public string ResumeButtonContent { get; set; }
    }
}
