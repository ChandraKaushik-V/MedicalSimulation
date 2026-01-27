using MedicalSimulation.Core.Services.DTOs;

namespace MedicalSimulation.Core.Services.Interfaces;

public interface IProfileService
{
    Task<UserProfileDto?> GetUserProfileAsync(string userId);
}
