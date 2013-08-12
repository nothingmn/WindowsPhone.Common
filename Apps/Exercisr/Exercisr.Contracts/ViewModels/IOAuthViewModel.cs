using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercisr.Contracts.Services;

namespace Exercisr.Contracts.ViewModels
{
    public interface IOAuthViewModel
    {
        IAccount ServiceAccount { get; set; }
        string Url { get; set; }

        void UpdateAccessToken(string token);
        void UpdateAccessCode(string code);

    }
}
