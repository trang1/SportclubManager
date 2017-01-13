using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Web.Mvc;

namespace SportclubManager.Models
{
    public partial class User
    {
        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }
            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var hasRole = rolesArray.Any(p => string.Compare(p, Role.RoleName, StringComparison.OrdinalIgnoreCase) == 0);
            return hasRole;
        }
    }

    public class Roles
    {
        public const string Administrator = "Administrator";
        public const string Coach = "Coach";
    }
}