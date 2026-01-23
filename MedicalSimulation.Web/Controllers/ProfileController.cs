using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using System.Security.Claims;

namespace MedicalSimulation.Web.Controllers;

[Authorize(Roles = "Student")]
public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get student information
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

        // Get ApplicationUser for additional details
        var user = await _context.Users
            .OfType<ApplicationUser>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        // Create a ViewModel combining both
        var profileData = new
        {
            FirstName = student?.FirstName ?? user?.FirstName ?? "",
            LastName = student?.LastName ?? user?.LastName ?? "",
            Email = student?.Email ?? user?.Email ?? "",
            PhoneNumber = student?.PhoneNumber ?? user?.PhoneNumber ?? "",
            StudentId = student?.StudentId ?? "N/A",
            CreatedAt = user?.CreatedAt ?? DateTime.UtcNow
        };

        ViewBag.ProfileData = profileData;

        return View();
    }
}
