using System.Diagnostics;
using JobPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IJobRepository _jobRepository;

        public HomeController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = await _jobRepository.GetAllJobAsync();
            return View(jobs.Take(6)); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
