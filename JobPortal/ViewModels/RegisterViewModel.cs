using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be of 8 length.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).*$",   ErrorMessage = "The field must contain at least one uppercase letter, one lowercase letter, and one number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
