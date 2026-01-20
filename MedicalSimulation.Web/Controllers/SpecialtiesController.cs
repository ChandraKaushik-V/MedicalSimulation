using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;

namespace MedicalSimulation.Web.Controllers;

[Authorize]
public class SpecialtiesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SpecialtiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var specialties = await _context.Specialties
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Include(s => s.Simulations)
            .ToListAsync();

        return View(specialties);
    }

    public async Task<IActionResult> Details(int id)
    {
        var specialty = await _context.Specialties
            .Include(s => s.Simulations.Where(sim => sim.IsActive))
            .FirstOrDefaultAsync(s => s.Id == id);

        if (specialty == null)
        {
            return NotFound();
        }

        return View(specialty);
    }
}
