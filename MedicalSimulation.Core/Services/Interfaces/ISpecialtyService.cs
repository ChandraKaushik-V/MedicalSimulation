using MedicalSimulation.Core.Models;

namespace MedicalSimulation.Core.Services.Interfaces;

public interface ISpecialtyService
{
    Task<List<Specialty>> GetAllActiveSpecialtiesAsync(int? limit = null);
    Task<Specialty?> GetSpecialtyDetailsAsync(int specialtyId);
}
