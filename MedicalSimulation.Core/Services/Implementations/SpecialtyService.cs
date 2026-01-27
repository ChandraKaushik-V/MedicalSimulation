using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using MedicalSimulation.Core.Services.Interfaces;

namespace MedicalSimulation.Core.Services.Implementations;

public class SpecialtyService : ISpecialtyService
{
    private readonly ApplicationDbContext _context;

    public SpecialtyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Specialty>> GetAllActiveSpecialtiesAsync(int? limit = null)
    {
        var query = _context.Specialties
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Include(s => s.Simulations);

        if (limit.HasValue)
        {
            return await query.Take(limit.Value).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<Specialty?> GetSpecialtyDetailsAsync(int specialtyId)
    {
        var specialty = await _context.Specialties
            .Include(s => s.Simulations.Where(sim => sim.IsActive))
            .FirstOrDefaultAsync(s => s.Id == specialtyId);

        return specialty;
    }
}
