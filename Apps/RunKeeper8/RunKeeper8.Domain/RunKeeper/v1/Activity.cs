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
    }




}