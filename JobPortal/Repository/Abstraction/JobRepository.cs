using JobPortal;
using JobPortal.Models;
using JobPortal.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _context;

    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobViewModel>> GetAllJobAsync()
    {
        try
        {
            return await _context.Jobs
                .Include(j => j.Company)
                .Select(j => new JobViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    PostedDate = j.PostedDate,
                    CompanyId = j.CompanyId,
                    CompanyName = j.Company.CompanyName
                })
                .ToListAsync();
        }
        catch (Exception)
        {
            return Enumerable.Empty<JobViewModel>();
        }
    }

    public async Task<JobViewModel?> GetJobByIdAsync(int id)
    {
        try
        {
            return await _context.Jobs
                .Include(j => j.Company)
                .Where(j => j.Id == id)
                .Select(j => new JobViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    PostedDate = j.PostedDate,
                    CompanyId = j.CompanyId,
                    CompanyName = j.Company.CompanyName
                })
                .FirstOrDefaultAsync();
        }

        catch (Exception)
        {
            return null;
        }
    }

    public async Task CreateJobAsync(JobViewModel vm)
    {
        try
        {
            var job = new Job
            {
                Title = vm.Title,
                Description = vm.Description,
                Location = vm.Location,
                CompanyId = vm.CompanyId,
                PostedDate = DateTime.Now
            };

            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateJobAsync(JobViewModel vm)
    {
        try
        {
            var job = await _context.Jobs.FindAsync(vm.Id);
            if (job == null) return;

            job.Title = vm.Title;
            job.Description = vm.Description;
            job.Location = vm.Location;
            job.CompanyId = vm.CompanyId;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UserOwnsCompanyAsync(int companyId, string userId)
    {
        return await _context.Companies
            .AnyAsync(c => c.Id == companyId && c.UserId == userId);
    }

    public async Task<PagedResult<Job>> GetAllJobsAsync(int page, int pageSize)
    {
        try
        {
            var query = _context.Jobs
                .Include(j => j.Company)
                .OrderByDescending(j => j.PostedDate);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Job>
            {
                Items = items,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
        catch (Exception)
        {
            return new PagedResult<Job>
            {
                Items = Enumerable.Empty<Job>(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = 0
            };
        }
    }

    public async Task DeleteJobAsync(int id)
    {
        try
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
