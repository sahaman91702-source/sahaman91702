using JobPortal.Models;
using JobPortal.ViewModels;

public interface IJobRepository
{
    Task<IEnumerable<JobViewModel>> GetAllJobAsync();
    Task<JobViewModel?> GetJobByIdAsync(int id);
    Task CreateJobAsync(JobViewModel jobViewModel);
    Task UpdateJobAsync(JobViewModel jobViewModel);
    Task<bool> UserOwnsCompanyAsync(int companyId, string userId);
    Task<PagedResult<Job>> GetAllJobsAsync(int page, int pageSize);
    Task DeleteJobAsync(int id);
}
