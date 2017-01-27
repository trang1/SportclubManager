using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Auth;

namespace SportclubManager.Models
{
    public partial class Member
    {
        public string IsActiveString
        {
            get { return IsActive.GetValueOrDefault(false) ? "Yes" : "No"; }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public string GroupName
        {
            get { return Group?.GroupName; }
        }

        public string DobString
        {
            get { return DOB?.ToShortDateString(); }
        }

        public string SelectedGroupValue
        {
            get
            {
                return Group == null ? null : Groups.First(r => r.Text == Group.GroupName).Value;
            }
            set
            {
                GroupID = string.IsNullOrEmpty(value) ? (int?)null : Convert.ToInt32(value);
            }
        }

        public IList<SelectListItem> Groups
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var groups = db.Groups.AsQueryable();

                if (CurrentUser.IsCoach)
                    groups = groups.Where(g => g.CoachID == CurrentUser.UserID);
                
                return groups.OrderBy(r => r.GroupName)
                    .Select(g => new SelectListItem() {Text = g.GroupName, Value = g.GroupID.ToString()})
                    .ToList();
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

        #region Birthday Properties
        public int BirthdateDay
        {
            get { return DOB.HasValue ? DOB.Value.Day : 1; }
            set
            {
                if (!DOB.HasValue)
                    DOB = new DateTime(1910, 1, value);
                else
                {
                    DOB = new DateTime(DOB.Value.Year, DOB.Value.Month, value);
                }
            }
        }

        public int BirthdateMonth
        {
            get { return DOB.HasValue ? DOB.Value.Month : 1; }
            set
            {
                if (!DOB.HasValue)
                    DOB = new DateTime(1910, value, 1);
                else
                {
                    DOB = new DateTime(DOB.Value.Year, value, DOB.Value.Day);
                }
            }
        }

        public int BirthdateYear
        {
            get { return DOB.HasValue ? DOB.Value.Year : 1; }
            set
            {
                if (!DOB.HasValue)
                    DOB = new DateTime(value, 1, 1);
                else
                {
                    DOB = new DateTime(value, DOB.Value.Month, DOB.Value.Day);
                }
            }
        }
        #endregion

        #region Date lists
        public IEnumerable<SelectListItem> BirthdateDaySelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = BirthdateDay == i
                    };
                }
            }
        }
        public IEnumerable<SelectListItem> BirthdateMonthSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = BirthdateMonth == i
                    };
                }
            }
        }
        public IEnumerable<SelectListItem> BirthdateYearSelectList
        {
            get
            {
                for (int i = 1920; i < DateTime.Now.Year; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = BirthdateYear == i
                    };
                }
            }
        }
        #endregion
    }
}