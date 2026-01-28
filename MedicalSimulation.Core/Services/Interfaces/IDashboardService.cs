using MedicalSimulation.Core.Services.DTOs;

namespace MedicalSimulation.Core.Services.Interfaces;

public interface IDashboardService
{
    Task<StudentDashboardDto> GetStudentDashboardAsync(string userId, int topRecentActivities = 10, int? filterBySimulationId = null);
    Task<InstructorDashboardDto> GetInstructorDashboardAsync(string instructorUserId, string? studentId = null, int pageNumber = 1, int pageSize = 50);
    Task<bool> GiveFeedbackAsync(int userProgressId, string instructorId, string feedbackText);
    Task<AttemptDetailDto?> GetAttemptDetailAsync(int attemptId, string userId);
}
