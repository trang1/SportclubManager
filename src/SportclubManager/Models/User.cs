using System;
using System.Linq;
using System.Web.Mvc;
using SportclubManager.Auth;

namespace SportclubManager.Models
{
    public partial class User
    {
        public bool IsCoach
        {
            get { return Role != null && Role.RoleName == Roles.Coach; }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }
            var rolesArray = roles.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
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