using System.Security.Claims;
using JobPortal.Interfaces;
using JobPortal.Models;
using JobPortal.Repositories;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Controllers
{
    [Authorize(Roles = "Company")]
    public class CompanyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IApplicationRepository _applicationRepository; 

        public CompanyController(AppDbContext context)
        {
            _context = context;
            _applicationRepository = new ApplicationRepository(_context);
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 5;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Companies
                .Include(c => c.Jobs)
                .Where(c => c.UserId == userId);

            var totalItems = await query.CountAsync();

            var companies = await query
                .OrderBy(c => c.CompanyName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<Company>
            {
                Items = companies,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(result); 
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = new Company
            {
                CompanyName = vm.CompanyName,
                UserId = userId
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (company == null)
                return NotFound();

            var vm = new CompanyViewModel
            {
                Id = company.Id,
                CompanyName = company.CompanyName
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == vm.Id && c.UserId == userId);

            if (company == null)
                return NotFound();

            company.CompanyName = vm.CompanyName;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Companies
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (company == null)
                return NotFound();

            return View(company);
        }

        public async Task<IActionResult> Applications(int page = 1)
        {
            const int pageSize = 10;
            var companyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _applicationRepository
                .GetApplicantsForCompanyAsync(companyUserId, page, pageSize);

            var vm = new PagedResult<JobApplicationViewModel>
            {
                Items = result.Items.Select(a => new JobApplicationViewModel
                {
                    JobTitle = a.Job.Title,
                    CompanyName = a.Job.Company.CompanyName,
                    FullName = a.FullName,
                    Email = a.Email,
                    Contact = a.ContactNumber,
                    CvPath = a.CvFilePath,
                    AppliedDate = a.AppliedDate
                }),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };

            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Companies
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (company == null)
                return NotFound();

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (company == null)
                return NotFound();

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
