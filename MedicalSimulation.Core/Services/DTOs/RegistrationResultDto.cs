using MedicalSimulation.Core.Models;

namespace MedicalSimulation.Core.Services.DTOs;

public class RegistrationResultDto
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public int? Id { get; set; }
}
