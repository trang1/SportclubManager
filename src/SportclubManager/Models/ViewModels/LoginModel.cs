using System.ComponentModel.DataAnnotations;

namespace SportclubManager.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}