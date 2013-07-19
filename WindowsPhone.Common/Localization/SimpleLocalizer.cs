using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone.Contracts.Localization;

namespace WindowsPhone.Common.Localization
{
    public class SimpleLocalizer : ILocalize 
    {
        public string Translate(string input, string locale)
        {
            return input;
        }
    }
}
