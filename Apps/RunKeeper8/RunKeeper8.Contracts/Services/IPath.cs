using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunKeeper8.Contracts.Services
{
    public interface IPath
    {

        int timestamp { get; set; }
        double altitude { get; set; }
        double longitude { get; set; }
        double latitude { get; set; }
        string type { get; set; }
    }
}
