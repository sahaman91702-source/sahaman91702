using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
