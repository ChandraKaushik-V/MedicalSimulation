using Microsoft.AspNetCore.Mvc;
using MedicalSimulation.Core.Services.Interfaces;

namespace MedicalSimulation.Web.Controllers;

public class HomeController : Controller
{
    private readonly ISpecialtyService _specialtyService;

    public HomeController(ISpecialtyService specialtyService)
    {
        _specialtyService = specialtyService;
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

            var specialties = await _specialtyService.GetAllActiveSpecialtiesAsync(limit: 6);
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
