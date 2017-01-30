using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Auth;

namespace SportclubManager.Models
{
    public class MemberEvidenceModel
    {
        public MultiSelectList GroupList
        {
            get
            {
                return new MultiSelectList(Groups, "GroupID", "GroupName");
            }
        }

        public IList<Group> Groups
        {
            get
            {
                var db = DependencyResolver.Current.GetService<SportclubManagerDataContext>();
                return db.Groups.Where(g => g.CoachID == CurrentUser.UserID).OrderBy(g => g.GroupName).ToList();
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

        public string CurrentDate { get; set; }

        public IList<MemberEvidenceWeek> MemberEvidences { get; set; } 
    }

    public class MemberEvidenceWeek : Dictionary<string,MemberEvidence>
    {
        public string MemberName { get; set; }
    }
}