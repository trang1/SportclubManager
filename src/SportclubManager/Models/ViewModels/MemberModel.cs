using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Auth;

namespace SportclubManager.Models
{
    public class MemberModel
    {
        public int MemberID { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [MaxLength(50, ErrorMessage = "Max length is 50 symbols")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [MaxLength(50, ErrorMessage = "Max length is 50 symbols")]
        public string LastName { get; set; }

        public DateTime? DOB { get; set; }

        [MaxLength(500, ErrorMessage = "Max length is 500 symbols")]
        public string Address { get; set; }

        [MaxLength(100, ErrorMessage = "Max length is 100 symbols")]
        public string Father { get; set; }

        [MaxLength(100, ErrorMessage = "Max length is 100 symbols")]
        public string FatherMail { get; set; }

        [MaxLength(50, ErrorMessage = "Max length is 50 symbols")]
        public string FatherPhoneNo { get; set; }

        [MaxLength(100, ErrorMessage = "Max length is 100 symbols")]
        public string Mother { get; set; }

        [MaxLength(100, ErrorMessage = "Max length is 100 symbols")]
        public string MotherMail { get; set; }

        [MaxLength(50, ErrorMessage = "Max length is 50 symbols")]
        public string MotherPhoneNo { get; set; }
        public string PhotoLocation { get; set; }
        public bool? IsActive { get; set; }
        public Group Group { get; set; }
        public int? GroupID { get; set; }

        public MemberModel()
        {
        }

        public MemberModel(Member member)
        {
            MemberID = member.MemberID;
            FirstName = member.FirstName;
            LastName = member.LastName;
            DOB = member.DOB;
            Address = member.Address;
            Father = member.Father;
            FatherMail = member.FatherMail;
            FatherPhoneNo = member.FatherPhoneNo;
            Mother = member.Mother;
            MotherMail = member.MotherMail;
            MotherPhoneNo = member.MotherPhoneNo;
            PhotoLocation = member.PhotoLocation;
            IsActive = member.IsActive;
            GroupID = member.GroupID;
            Group = member.Group;
        }

        public Member GetMember()
        {
            return new Member()
            {
                MemberID = MemberID,
                FirstName = FirstName,
                LastName = LastName,
                DOB = DOB,
                Address = Address,
                Father = Father,
                FatherMail = FatherMail,
                FatherPhoneNo = FatherPhoneNo,
                Mother = Mother,
                MotherMail = MotherMail,
                MotherPhoneNo = MotherPhoneNo,
                PhotoLocation = PhotoLocation,
                IsActive = IsActive,
                GroupID = GroupID
            };
        }
        public IList<SelectListItem> GroupList
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                var groups = db.Groups.AsQueryable();

                if (UserProvider.CurrentUser.IsCoach)
                    groups = groups.Where(g => g.CoachID == UserProvider.CurrentUser.UserID);

                return groups.OrderBy(r => r.GroupName)
                    .Select(g => new SelectListItem() { Text = g.GroupName, Value = g.GroupID.ToString() })
                    .ToList();
            }
        }

        [Required(ErrorMessage = "Please select a group")]
        public string SelectedGroupValue
        {
            get
            {
                return Group == null ? null : GroupList.First(r => r.Text == Group.GroupName).Value;
            }
            set
            {
                GroupID = string.IsNullOrEmpty(value) ? (int?)null : Convert.ToInt32(value);
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