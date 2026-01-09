using JobPortal.Interfaces;
using JobPortal.Models;
using JobPortal.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task ApplyAsync(ApplyJobViewModel vm, string userId, string cvPath)
        {
            try
            {
                var application = new JobApplication
                {
                    JobId = vm.JobId,
                    JobSeekerId = userId,
                    FullName = vm.FullName,
                    Email = vm.Email,
                    ContactNumber = vm.ContactNumber,
                    CvFilePath = cvPath
                };

                _context.JobApplications.Add(application);
                await _context.SaveChangesAsync();
            }
            catch (Exception er)
            {
                Console.WriteLine($"An error occurred while applying for the job: {er.Message}");
                throw;
            }
        }

        public async Task<PagedResult<JobApplication>> GetApplicationsByUserAsync(string userId, int page, int pageSize)
        {
            try
            {
                var query = _context.JobApplications
                    .Include(a => a.Job)
                    .ThenInclude(j => j.Company)
                    .Where(a => a.JobSeekerId == userId)
                    .OrderByDescending(a => a.AppliedDate);

                var totalItems = await query.CountAsync();

                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResult<JobApplication>
                {
                    Items = items,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };
            }
            catch (Exception)
            {
                return new PagedResult<JobApplication>
                {
                    Items = new List<JobApplication>(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = 0
                };
            }
        }

        public async Task<PagedResult<JobApplication>> GetApplicantsForCompanyAsync(string companyUserId, int page, int pageSize)
        {
            try
            {
                var query = _context.JobApplications
                    .Include(a => a.Job)
                    .ThenInclude(j => j.Company)
                    .Where(a => a.Job.Company.UserId == companyUserId)
                    .OrderByDescending(a => a.AppliedDate);

                var totalItems = await query.CountAsync();

                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResult<JobApplication>
                {
                    Items = items,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };
            }
            catch (Exception)
            {
                return new PagedResult<JobApplication>
                {
                    Items = new List<JobApplication>(),
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = 0
                };
            }
        }
    }
}
