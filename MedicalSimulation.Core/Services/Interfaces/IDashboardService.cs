using MedicalSimulation.Core.Services.DTOs;

namespace MedicalSimulation.Core.Services.Interfaces;

public interface IDashboardService
{
    Task<StudentDashboardDto> GetStudentDashboardAsync(string userId);
    Task<InstructorDashboardDto> GetInstructorDashboardAsync(string instructorUserId, string? studentId = null);
    Task<bool> GiveFeedbackAsync(int userProgressId, string instructorId, string feedbackText);
    Task<AttemptDetailDto?> GetAttemptDetailAsync(int attemptId, string userId);
}
