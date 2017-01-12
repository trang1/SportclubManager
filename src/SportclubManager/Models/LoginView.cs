using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportclubManager.Models
{

    public class LoginView
    {
        [Required(ErrorMessage = "Введите Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}