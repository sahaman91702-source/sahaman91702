using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
