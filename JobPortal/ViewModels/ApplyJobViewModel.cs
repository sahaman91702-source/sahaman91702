using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JobPortal.ViewModels
{
    public class ApplyJobViewModel
    {
        public int JobId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MinLength(5, ErrorMessage = "Full Name must be at least 5 characters long.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Length(10, 10, ErrorMessage = "Contact Number must be of 10 digits.")]
        public string ContactNumber { get; set; }

        [Required]
        public IFormFile CvFile { get; set; }
    }
}
