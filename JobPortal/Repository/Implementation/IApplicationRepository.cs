using System.Threading.Tasks;
using JobPortal.Models;
using JobPortal.ViewModels;

namespace JobPortal.Interfaces
{
    public interface IApplicationRepository
    {
        Task ApplyAsync(ApplyJobViewModel vm, string userId, string cvPath);

        Task<PagedResult<JobApplication>> GetApplicationsByUserAsync(
            string userId, int page, int pageSize);

        Task<PagedResult<JobApplication>> GetApplicantsForCompanyAsync(
            string companyUserId, int page, int pageSize);
    }

}
