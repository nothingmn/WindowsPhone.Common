using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercisr.Contracts.Services;

namespace Exercisr.Domain.RunKeeper.v1
{

    public class Path : IPath
    {
        public int timestamp { get; set; }
        public double altitude { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string type { get; set; }
    }
}
