using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhone.Contracts.Membership
{
    public interface IUser
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string DisplayName { get; set; }
        string AvatarUrl { get; set; }
        string Email { get; set; }
        string RunkeeperToken { get; set; }

        void FromIUser(IUser source);
    }
}
