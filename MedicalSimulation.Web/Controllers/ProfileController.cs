using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalSimulation.Core.Services.Interfaces;
using System.Security.Claims;

namespace MedicalSimulation.Web.Controllers;

[Authorize(Roles = "Student")]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var profileData = await _profileService.GetUserProfileAsync(userId);

        if (profileData == null)
        {
            return NotFound();
        }

        ViewBag.ProfileData = profileData;

        return View();
    }
}
