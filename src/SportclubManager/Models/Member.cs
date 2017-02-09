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
    }
}