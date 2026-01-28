using MedicalSimulation.Core.Models;

namespace MedicalSimulation.Core.Services.DTOs;

public class StudentDashboardDto
{
    public int CompletedSimulations { get; set; }
    public int TotalAttempts { get; set; }
    public double AverageScore { get; set; }
    public List<UserProgress> RecentProgress { get; set; } = new();
    public List<Feedback> Feedbacks { get; set; } = new();
    public List<Instructor> Instructors { get; set; } = new();
    public List<PerformanceTrendDto> PerformanceTrend { get; set; } = new();
}
