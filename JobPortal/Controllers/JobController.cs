using System.Security.Claims;
using JobPortal.Models;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Interfaces;

namespace JobPortal.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly ILogger<JobController> _logger;
        private readonly IJobRepository _jobRepository;
        private readonly IApplicationRepository _applicationRepository;

        public JobController(ILogger<JobController> logger, IJobRepository jobRepository, IApplicationRepository applicationRepository)
        {
            _logger = logger;
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 8;

            var result = await _jobRepository.GetAllJobsAsync(page, pageSize);

            var vm = new PagedResult<JobViewModel>
            {
                Items = result.Items.Select(j => new JobViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Location = j.Location,
                    CompanyName = j.Company.CompanyName,
                    PostedDate = j.PostedDate
                }),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);
            if (job == null)
                return NotFound();

            return View(job);
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Create(int companyId)
        {
            var vm = new JobViewModel
            {
                CompanyId = companyId
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var ownsCompany = await _jobRepository.UserOwnsCompanyAsync(vm.CompanyId, userId);

            if (!ownsCompany)
            
                return Forbid();

            await _jobRepository.CreateJobAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Edit(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);
            if (job == null)
                return NotFound();

            return View(job);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _jobRepository.UpdateJobAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);
            if (job == null)
                return NotFound();

            return View(job);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Company")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _jobRepository.DeleteJobAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyApplications(int page = 1)
        {
            const int pageSize = 10;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _applicationRepository
                .GetApplicationsByUserAsync(userId, page, pageSize);

            var vm = new PagedResult<JobApplicationViewModel>
            {
                Items = result.Items.Select(a => new JobApplicationViewModel
                {
                    JobTitle = a.Job.Title,
                    CompanyName = a.Job.Company.CompanyName,
                    CvPath = a.CvFilePath,
                    AppliedDate = a.AppliedDate
                }),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };

            return View(vm);
        }

        [Authorize(Roles = "User")]
        public IActionResult Apply(int id)
        {
            var vm = new ApplyJobViewModel
            {
                JobId = id
            };
            return View(vm);
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(ApplyJobViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cvs");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{vm.CvFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await vm.CvFile.CopyToAsync(stream);
            }

            var dbPath = "/cvs/" + fileName;

            await _applicationRepository.ApplyAsync(vm, userId, dbPath);

            return RedirectToAction(nameof(MyApplications));
        }

    }
}
