using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        [MinLength(15)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        public DateTime PostedDate { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; }
    }
}
