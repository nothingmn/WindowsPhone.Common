using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts.Core;

namespace WindowsPhone.Contracts
{
    public interface IApplication
    {
        string Locale { get; set; }
        IVersion AppVersion { get; set; }
        string Name { get; set; }
        string ToString();
    }
}