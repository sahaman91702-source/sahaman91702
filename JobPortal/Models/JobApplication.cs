using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }
        public Job Job { get; set; }

        [Required]
        public string JobSeekerId { get; set; }
        public ApplicationUser JobSeeker { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [MinLength(5, ErrorMessage = "Full Name must be at least 5 characters long.")]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Number")]
        [Phone(ErrorMessage = "Please enter a valid contact number.")]
        [Length(10, 10, ErrorMessage = "Contact Number must be of 10 digits.")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "CV File Path")]
        public string CvFilePath { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;
    }
}
