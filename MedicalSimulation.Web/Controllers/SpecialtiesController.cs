using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalSimulation.Core.Services.Interfaces;

namespace MedicalSimulation.Web.Controllers;

[Authorize]
public class SpecialtiesController : Controller
{
    private readonly ISpecialtyService _specialtyService;

    public SpecialtiesController(ISpecialtyService specialtyService)
    {
        _specialtyService = specialtyService;
    }

    public async Task<IActionResult> Index()
    {
        var specialties = await _specialtyService.GetAllActiveSpecialtiesAsync();
        return View(specialties);
    }

    public async Task<IActionResult> Details(int id)
    {
        var specialty = await _specialtyService.GetSpecialtyDetailsAsync(id);

        if (specialty == null)
        {
            return NotFound();
        }

        return View(specialty);
    }
}
