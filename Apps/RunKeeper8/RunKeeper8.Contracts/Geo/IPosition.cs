using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunKeeper8.Contracts.Geo
{
    public interface IPosition
    {
        /// <summary>
        /// The latitude of the position.
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// The longitude of the position.
        /// </summary>
        double Longitude { get; set; }
    }
}
