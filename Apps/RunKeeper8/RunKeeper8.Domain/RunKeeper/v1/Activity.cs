using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.Services;

namespace RunKeeper8.Domain.RunKeeper.v1
{


    public class Activity : IActivity
    {
        public string type { get; set; }
        public string equipment { get; set; }
        public string start_time { get; set; }
        public string notes { get; set; }
        public IList<IPath> path { get; set; }
        public bool post_to_facebook { get; set; }
        public bool post_to_twitter { get; set; }

        public string secondary_type { get; set; }
        public double total_distance { get; set; }
        public double duration { get; set; }
        public double total_calories { get; set; }
        public bool detect_pauses { get; set; }

    }




}