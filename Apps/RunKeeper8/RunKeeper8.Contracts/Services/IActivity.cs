﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace RunKeeper8.Contracts.Services
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
    }
}