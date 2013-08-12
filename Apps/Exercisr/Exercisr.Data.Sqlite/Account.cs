using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercisr.Contracts.Services;

namespace WindowsPhone.Data.DTO
{

    public class Account : IAccount
    {
        public string ClientID { get; set; }

        public string ClientSecret { get; set; }
        public string AuthorizationURL { get; set; }
        public string AccessTokenURL { get; set; }
        public string DeAuthorizationURL { get; set; }
        public string RedirectURL { get; set; }
        public string Code { get; set; }
        public string AccessToken { get; set; }
        public string RecordActivityUrl { get; set; }

        public string AuthorizationEndPoint()
        {
            throw new NotImplementedException();
        }

        public string TokenEndPoint()
        {
            throw new NotImplementedException();
        }

        public string TokenParameters()
        {
            throw new NotImplementedException();
        }

        public string RecordActivityEndPoint()
        {
            throw new NotImplementedException();
        }



        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}