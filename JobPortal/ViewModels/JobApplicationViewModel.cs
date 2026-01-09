namespace JobPortal.ViewModels
{
    public class JobApplicationViewModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string CvPath { get; set; }

        public DateTime AppliedDate { get; set; }
    }

}
