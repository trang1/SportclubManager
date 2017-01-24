﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportclubManager.Models
{
    public partial class Group
    {
        public IList<SelectListItem> Coaches
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                return
                    db.Users.Where(u => u.Role.RoleName == Roles.Coach)
                        .OrderBy(u => u.FirstName)
                        .Select(u => new SelectListItem() {Text = u.FullName, Value = u.UserID.ToString()})
                        .ToList();
            }
        }

        public string SelectedCoachValue
        {
            get
            {
                return User == null ? null : Coaches.First(c => c.Text == User.FullName).Value;
                
            }
            set
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var user = db.Users.FirstOrDefault(r => r.UserID == Convert.ToInt32(value));
                User = user;
            }
        }

        public string CoachFullName
        {
            get { return User?.FullName; }
        }
    }
}