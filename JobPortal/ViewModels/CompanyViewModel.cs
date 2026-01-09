using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
    }
}
