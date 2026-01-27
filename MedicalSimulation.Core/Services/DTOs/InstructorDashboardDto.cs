using MedicalSimulation.Core.Models;

namespace MedicalSimulation.Core.Services.DTOs;

public class InstructorDashboardDto
{
    public List<UserProgress> AllProgress { get; set; } = new();
    public List<Student> Students { get; set; } = new();
    public List<Feedback> Feedbacks { get; set; } = new();
    public List<StudentSummary>? StudentSummaries { get; set; }
    public string? SelectedStudentId { get; set; }
    public Student? SelectedStudent { get; set; }
}

public class StudentSummary
{
    public Student Student { get; set; } = null!;
    public int AttemptCount { get; set; }
    public int CompletedCount { get; set; }
    public double AverageScore { get; set; }
    public int PendingFeedback { get; set; }
}
