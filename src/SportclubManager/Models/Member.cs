using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportclubManager.Models
{
    public partial class Member
    {
        public string IsActiveString
        {
            get { return IsActive.GetValueOrDefault(false) ? "No" : "Yes"; }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}