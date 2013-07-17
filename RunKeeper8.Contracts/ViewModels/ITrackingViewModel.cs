using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunKeeper8.Contracts.ViewModels
{
    public interface ITrackingViewModel
    {
        double Distance { get; set; }
        double Time { get; set; }
        double Calories { get; set; }
        double Pace { get; set; }

        string StartButtonContent { get; set; }
        string ResumeButtonContent { get; set; }

    }
}
