using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Please enter Document Name")]
        public string DocumentNameProxy
        {
            get { return DocumentName; }
            set { DocumentName = value; }
        }
    }
}