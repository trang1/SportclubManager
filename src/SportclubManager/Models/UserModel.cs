﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;

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
        public EntitySet<Group> Groups { get; set; } 
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
            Groups = user.Groups;
            Role = user.Role;
            RoleID = user.RoleID;
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
                RoleID = RoleID
            };
        }

        public List<string> SelectedGroups
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var groups = db.Groups.Where(g => g.CoachID == UserID).ToList();
                return groups.Select(g => g.GroupID.ToString()).ToList();
            }
            set
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var groups = db.Groups.Where(g => value.Contains(g.GroupID.ToString())).ToList();
                groups.ForEach(g => g.CoachID = UserID);
                db.Groups.Context.SubmitChanges();
            }
        }

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