using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercisr.Contracts.Configuration
{

    public delegate void SettingsChanged(ISettings settings, string propertyName);

    public interface ISettings
    {
        event SettingsChanged OnSettingsChanged;

        bool IsMetric { get; set; }
        bool AutoPostToRunKeeper { get; set; }

        void Save();
        void Load();
    }
}
