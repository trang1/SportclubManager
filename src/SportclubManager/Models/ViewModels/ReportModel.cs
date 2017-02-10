using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Auth;

namespace SportclubManager.Models
{
    public class ReportModel
    {
        public MultiSelectList GroupList
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var groups = db.Groups.Where(g => g.CoachID == CurrentUser.UserID).OrderBy(g => g.GroupName).ToList();
                return new MultiSelectList(groups, "GroupID", "GroupName");
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

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        [Required(ErrorMessage = "Please select a group")]
        public int? GroupID { get; set; }

        #region DateFrom Properties

        public int DateFromDay
        {
            get { return DateFrom.Day; }
            set { DateFrom = new DateTime(DateFrom.Year, DateFrom.Month, value); }
        }
        public int DateFromMonth
        {
            get { return DateFrom.Month; }
            set { DateFrom = new DateTime(DateFrom.Year, value, DateFrom.Day); }
        }
        public int DateFromYear
        {
            get { return DateFrom.Year; }
            set { DateFrom = new DateTime(value, DateFrom.Month, DateFrom.Day); }
        }

        #endregion

        #region DateTo Properties

        public int DateToDay
        {
            get { return DateTo.Day; }
            set { DateTo = new DateTime(DateTo.Year, DateTo.Month, value); }
        }
        public int DateToMonth
        {
            get { return DateTo.Month; }
            set { DateTo = new DateTime(DateTo.Year, value, DateTo.Day); }
        }
        public int DateToYear
        {
            get { return DateTo.Year; }
            set { DateTo = new DateTime(value, DateTo.Month, DateTo.Day); }
        }

        #endregion

        #region DateFrom lists
        public IEnumerable<SelectListItem> DateFromDaySelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = DateFromDay == i
                    };
                }
            }
        }
        public IEnumerable<SelectListItem> DateFromMonthSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = DateFromMonth == i
                    };
                }
            }
        }
        public IEnumerable<SelectListItem> DateFromYearSelectList
        {
            get
            {
                for (int i = 1920; i <= DateTime.Now.Year; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = DateFromYear == i
                    };
                }
            }
        }
        #endregion

        #region DateTo lists
        public IEnumerable<SelectListItem> DateToDaySelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = DateToDay == i
                    };
                }
            }
        }
        public IEnumerable<SelectListItem> DateToMonthSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = DateToMonth == i
                    };
                }
            }
        }
        public IEnumerable<SelectListItem> DateToYearSelectList
        {
            get
            {
                for (int i = 1920; i <= DateTime.Now.Year; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = DateToYear == i
                    };
                }
            }
        }
        #endregion
    }
}