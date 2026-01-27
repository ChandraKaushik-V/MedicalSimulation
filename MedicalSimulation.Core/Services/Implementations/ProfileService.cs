using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using MedicalSimulation.Core.Services.DTOs;
using MedicalSimulation.Core.Services.Interfaces;

namespace MedicalSimulation.Core.Services.Implementations;

public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _context;

    public ProfileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
    {
        // Get student information
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

        // Get ApplicationUser for additional details
        var user = await _context.Users
            .OfType<ApplicationUser>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (student == null && user == null)
        {
            return null;
        }

        return new UserProfileDto
        {
            FirstName = student?.FirstName ?? user?.FirstName ?? "",
            LastName = student?.LastName ?? user?.LastName ?? "",
            Email = student?.Email ?? user?.Email ?? "",
            PhoneNumber = student?.PhoneNumber ?? user?.PhoneNumber ?? "",
            StudentId = student?.StudentId ?? "N/A",
            CreatedAt = user?.CreatedAt ?? DateTime.UtcNow
        };
    }
}
