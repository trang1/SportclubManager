using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportclubManager.Models
{
    public partial class Document
    {
        public string UserFullName
        {
            get { return User?.FullName; }
        }
    }
}