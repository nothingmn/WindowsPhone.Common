using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Exercisr.Contracts.Services
{
    public interface IActivity
    {
        string type { get; set; }
        string equipment { get; set; }
        string start_time { get; set; }
        string notes { get; set; }
        IList<IPath> path { get; set; }
        bool post_to_facebook { get; set; }
        bool post_to_twitter { get; set; }

        string secondary_type { get; set; }
        double total_distance { get; set; }
        double duration { get; set; }
        double total_calories { get; set; }
        bool detect_pauses { get; set; }

    }
}
