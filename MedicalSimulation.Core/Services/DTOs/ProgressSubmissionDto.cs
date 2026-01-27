namespace MedicalSimulation.Core.Services.DTOs;

public class ProgressSubmissionDto
{
    public int Score { get; set; }
    public bool IsCompleted { get; set; }
    public int TimeSpentSeconds { get; set; }
    public Dictionary<string, object>? Feedback { get; set; }
}
