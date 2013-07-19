using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhone.Contracts.Localization
{
    public interface ILocalize
    {
        string Translate(string input, string locale);
    }
}
