using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;

namespace MedicalSimulation.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            // Redirect instructors to their dashboard
            if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("InstructorIndex", "Dashboard");
            }
            
            var specialties = await _context.Specialties
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .Take(6)
                .ToListAsync();

            ViewBag.Specialties = specialties;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
