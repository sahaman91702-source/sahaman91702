using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class CompanyDashboardViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public IEnumerable<JobViewModel> Jobs { get; set; } = new List<JobViewModel>();
    }
}
