using MedicalSimulation.Core.Models;

namespace MedicalSimulation.Core.Services.DTOs;

public class SimulationDetailDto
{
    public Simulation Simulation { get; set; } = null!;
    public List<UserProgress> UserProgress { get; set; } = new();
    public int BestScore { get; set; }
    public int Attempts { get; set; }
}
