using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts;
using WindowsPhone.Contracts.Core;

namespace WindowsPhone.Common
{
    public class Application : IApplication
    {
        public Application()
        {
            Locale = CultureInfo.CurrentUICulture.Name;
            this.AppVersion = new AppVersion();
            this.Name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
        }
        public string Locale { get; set; }
        public IVersion AppVersion { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, AppVersion.ToString());
        }
    }
}
