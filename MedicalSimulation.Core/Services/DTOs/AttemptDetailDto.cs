using MedicalSimulation.Core.Models;

namespace MedicalSimulation.Core.Services.DTOs;

public class AttemptDetailDto
{
    public UserProgress Attempt { get; set; } = null!;
    public Feedback? Feedback { get; set; }
    public Instructor? Instructor { get; set; }
    public List<UserProgress> AllAttempts { get; set; } = new();
    public double AverageScoreForSimulation { get; set; }
}
