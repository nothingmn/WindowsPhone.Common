using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using WindowsPhone.Contracts.Membership;

namespace WindowsPhone.Data.DTO
{
    public class User : IUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }

        public string RunkeeperToken { get; set; }

        public void FromIUser(IUser source)
        {
            this.Id = source.Id;
            this.FirstName = source.FirstName;
            this.LastName = source.LastName;
            this.DisplayName = source.DisplayName;
            this.AvatarUrl = source.AvatarUrl;
            this.Email = source.AvatarUrl;
            this.RunkeeperToken = source.RunkeeperToken;
        }
    }
}
