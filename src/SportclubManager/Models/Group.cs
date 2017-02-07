using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                CoachID = string.IsNullOrEmpty(value) ? (int?)null : Convert.ToInt32(value);
            }
        }

        public string CoachFullName
        {
            get { return User?.FullName; }
        }

        [Required(ErrorMessage = "Please enter Group Name")]
        public string GroupNameProxy
        {
            get { return GroupName; }
            set { GroupName = value; }
        }
    }
}