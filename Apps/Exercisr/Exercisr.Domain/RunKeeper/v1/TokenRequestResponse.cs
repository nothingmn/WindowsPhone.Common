using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercisr.Domain.RunKeeper.v1
{
    public class TokenRequestResponse
    {
        public string token_type { get; set; }
        public string access_token { get; set; }

    }

}
