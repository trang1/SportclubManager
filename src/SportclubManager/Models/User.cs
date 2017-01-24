using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Web.Mvc;
using SportclubManager.Auth;

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

        public IList<SelectListItem> Roles
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                return db.Roles.OrderBy(r => r.RoleName)
                        .Select(r => new SelectListItem() { Text = r.RoleName, Value = r.RoleID.ToString() })
                        .ToList();
            }
        }

        public MultiSelectList GroupList
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var groups = db.Groups.OrderBy(g=>g.GroupName).ToList();
                return new MultiSelectList(groups, "GroupID", "GroupName");
            }
        }

        public List<string> SelectedGroups
        {
            get
            {
                return Groups.Select(g=>g.GroupID.ToString()).ToList(); 
            }
            set
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var groups = db.Groups.Where(g => value.Contains(g.GroupID.ToString())).ToList();
                groups.ForEach(g=>g.CoachID = UserID);
                db.Groups.Context.SubmitChanges();
            }
        } 

        public string SelectedRoleValue
        {
            get
            {
                return Role == null ? null : Roles.First(r => r.Text == Role.RoleName).Value;
            }
            set
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var role = db.Roles.FirstOrDefault(r => r.RoleID == Convert.ToInt32(value));
                Role = role;
            }
        }

        public User CurrentUser
        {
            get
            {
                var auth = DependencyResolver.Current.GetService<IAuthentication>();
                return ((IUserIdentity)auth.CurrentUser.Identity).User;
            }
        }

        public bool IsCoach
        {
            get { return Role != null && Role.RoleName == Models.Roles.Coach; }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }

    public class Roles
    {
        public const string Administrator = "Administrator";
        public const string Coach = "Coach";
    }
}