using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SportclubManager.Models;

namespace SportclubManager.Auth
{
    public interface IUserIdentity : IIdentity
    {
        User User { get; set; }
    }
}
