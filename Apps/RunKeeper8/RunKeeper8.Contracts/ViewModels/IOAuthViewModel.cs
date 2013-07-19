using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunKeeper8.Contracts.Services;

namespace RunKeeper8.Contracts.ViewModels
{
    public interface IOAuthViewModel
    {
        IAccount ServiceAccount { get; set; }

        string Url { get; set; }
        
    }
}
