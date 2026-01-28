namespace MedicalSimulation.Core.Services.DTOs;

public class PerformanceTrendDto
{
    public string MonthYear { get; set; } = string.Empty;
    public decimal AverageScore { get; set; }
    public int AttemptCount { get; set; }
}
