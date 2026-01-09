using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class JobViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(7, ErrorMessage = "Title must be at least of 7 characters long.")]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Display(Name = "Posted Date")]
        public DateTime PostedDate { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public string? CompanyName { get; set; }
    }
}
