using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using System.Security.Claims;

namespace MedicalSimulation.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userProgress = await _context.UserProgress
            .Include(up => up.Simulation)
            .ThenInclude(s => s!.Specialty)
            .Where(up => up.UserId == userId)
            .OrderByDescending(up => up.StartedAt)
            .ToListAsync();

        var completedSimulations = userProgress
            .Where(up => up.IsCompleted)
            .GroupBy(up => up.SimulationId)
            .Count();

        var totalAttempts = userProgress.Count;
        var averageScore = userProgress.Any() ? userProgress.Average(up => up.Score) : 0;

        ViewBag.CompletedSimulations = completedSimulations;
        ViewBag.TotalAttempts = totalAttempts;
        ViewBag.AverageScore = averageScore;
        ViewBag.RecentProgress = userProgress.Take(10).ToList();

        return View();
    }

    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> InstructorIndex()
    {
        var allProgress = await _context.UserProgress
            .Include(up => up.Simulation)
            .Include(up => up.User)
            .OrderByDescending(up => up.StartedAt)
            .ToListAsync();

        return View(allProgress);
    }
}
