using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;
using Ninject.Infrastructure.Language;

namespace SportclubManager.Models
{
    public class UserModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter User Login")]
        public string UserLogin { get; set; }

        [Required(ErrorMessage = "Please enter User Password")]
        public string UserPassword { get; set; }

        public bool IsCoach { get; set; }
        public Role Role { get; set; }
        public int? RoleID { get; set; }

        public UserModel()
        {
        }

        public UserModel(User user)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserLogin = user.UserLogin;
            UserPassword = user.UserPassword;
            IsCoach = user.IsCoach;
            Role = user.Role;
            RoleID = user.RoleID;
            SelectedGroups = user.Groups.Select(g=>g.GroupID.ToString()).ToList();
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
                var groups = db.Groups.OrderBy(g => g.GroupName).ToList();
                return new MultiSelectList(groups, "GroupID", "GroupName");
            }
        }
        
        public User GetUser()
        {
            return new User
            {
                UserID = UserID,
                FirstName = FirstName,
                LastName = LastName,
                UserLogin = UserLogin,
                UserPassword = UserPassword,
                RoleID = RoleID,
                SelectedGroups = SelectedGroups
            };
        }

        public List<string> SelectedGroups { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        public string SelectedRoleValue
        {
            get
            {
                return Role == null ? null : Roles.First(r => r.Text == Role.RoleName).Value;
            }
            set
            {
                RoleID = string.IsNullOrEmpty(value) ? (int?)null : Convert.ToInt32(value);
            }
        }
    }
}