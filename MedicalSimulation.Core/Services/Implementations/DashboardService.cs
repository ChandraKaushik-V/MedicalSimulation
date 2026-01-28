using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using MedicalSimulation.Core.Services.DTOs;
using MedicalSimulation.Core.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MedicalSimulation.Core.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentDashboardDto> GetStudentDashboardAsync(string userId, int topRecentActivities = 10, int? filterBySimulationId = null)
    {
        var result = new StudentDashboardDto();

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@TopRecentActivities", topRecentActivities),
                new SqlParameter("@FilterBySimulationId", filterBySimulationId ?? (object)DBNull.Value)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_GetStudentDashboardData";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();

            // Result Set 1: Summary Stats
            if (await reader.ReadAsync())
            {
                result.CompletedSimulations = reader.GetInt32(reader.GetOrdinal("CompletedSimulations"));
                result.TotalAttempts = reader.GetInt32(reader.GetOrdinal("TotalAttempts"));
                result.AverageScore = (double)reader.GetDecimal(reader.GetOrdinal("AverageScore"));
            }

            // Result Set 2: Recent Activities - Get IDs only
            await reader.NextResultAsync();
            var progressIds = new List<int>();
            while (await reader.ReadAsync())
            {
                progressIds.Add(reader.GetInt32(reader.GetOrdinal("ProgressId")));
            }

            // Result Set 3: Feedback List - Get IDs only
            await reader.NextResultAsync();
            var feedbackIds = new List<int>();
            while (await reader.ReadAsync())
            {
                feedbackIds.Add(reader.GetInt32(reader.GetOrdinal("FeedbackId")));
            }

            // Result Set 4: Performance Trend (Last 6 months)
            await reader.NextResultAsync();
            var performanceTrend = new List<PerformanceTrendDto>();
            while (await reader.ReadAsync())
            {
                var trend = new PerformanceTrendDto
                {
                    MonthYear = reader.GetString(reader.GetOrdinal("MonthYear")),
                    AverageScore = reader.GetDecimal(reader.GetOrdinal("AverageScore")),
                    AttemptCount = reader.GetInt32(reader.GetOrdinal("AttemptCount"))
                };
                performanceTrend.Add(trend);
            }
            result.PerformanceTrend = performanceTrend;

            await _context.Database.CloseConnectionAsync();

            // Now load full entities using EF Core
            if (progressIds.Any())
            {
                result.RecentProgress = await _context.UserProgress
                    .Include(up => up.Simulation)
                    .ThenInclude(s => s!.Specialty)
                    .Where(up => progressIds.Contains(up.Id))
                    .OrderByDescending(up => up.StartedAt)
                    .ToListAsync();
            }

            if (feedbackIds.Any())
            {
                result.Feedbacks = await _context.Feedbacks
                    .Include(f => f.Instructor)
                    .Include(f => f.Simulation)
                    .Where(f => feedbackIds.Contains(f.Id))
                    .OrderByDescending(f => f.CreatedAt)
                    .ToListAsync();

                // Load instructor details
                var instructorIds = result.Feedbacks.Select(f => f.InstructorId).Distinct().ToList();
                result.Instructors = await _context.Instructors
                    .Include(i => i.Specialization)
                    .Where(i => instructorIds.Contains(i.ApplicationUserId))
                    .ToListAsync();
            }
        }
        catch
        {
            await _context.Database.CloseConnectionAsync();
            throw;
        }

        return result;
    }

    public async Task<InstructorDashboardDto> GetInstructorDashboardAsync(string instructorUserId, string? studentId = null, int pageNumber = 1, int pageSize = 50)
    {
        var result = new InstructorDashboardDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SelectedStudentId = studentId
        };

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@InstructorId", instructorUserId),
                new SqlParameter("@FilterByStudentId", studentId ?? (object)DBNull.Value),
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_GetInstructorDashboardData";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();

            // Result Set 1: Student Summaries
            var studentSummaries = new List<StudentSummary>();
            var studentIds = new List<string>();
            while (await reader.ReadAsync())
            {
                var currentStudentId = reader.GetString(reader.GetOrdinal("StudentId"));
                studentIds.Add(currentStudentId);
                
                var summary = new StudentSummary
                {
                    Student = null!, // Will be populated below
                    AttemptCount = reader.GetInt32(reader.GetOrdinal("TotalAttempts")),
                    CompletedCount = reader.GetInt32(reader.GetOrdinal("CompletedCount")),
                    AverageScore = (double)reader.GetDecimal(reader.GetOrdinal("AvgScore")),
                    PendingFeedback = reader.GetInt32(reader.GetOrdinal("PendingFeedbackCount"))
                };
                studentSummaries.Add(summary);
            }

            // Result Set 2: Recent Attempts (Paginated) - Get IDs only
            await reader.NextResultAsync();
            var progressIds = new List<int>();
            while (await reader.ReadAsync())
            {
                progressIds.Add(reader.GetInt32(reader.GetOrdinal("ProgressId")));
            }

            // Result Set 3: Existing Feedback - Get IDs only
            await reader.NextResultAsync();
            var feedbackIds = new List<int>();
            while (await reader.ReadAsync())
            {
                feedbackIds.Add(reader.GetInt32(reader.GetOrdinal("FeedbackId")));
            }

            // Result Set 4: Pending Feedback Count & Total Records
            await reader.NextResultAsync();
            if (await reader.ReadAsync())
            {
                result.PendingFeedbackCount = reader.GetInt32(reader.GetOrdinal("PendingFeedbackCount"));
                result.TotalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
            }

            await _context.Database.CloseConnectionAsync();

            // Now load full entities using EF Core
            if (studentIds.Any())
            {
                result.Students = await _context.Students
                    .Where(s => studentIds.Contains(s.ApplicationUserId))
                    .ToListAsync();

                // Populate student summaries with actual student objects
                for (int i = 0; i < studentSummaries.Count && i < result.Students.Count; i++)
                {
                    studentSummaries[i].Student = result.Students[i];
                }
                result.StudentSummaries = studentSummaries;
            }

            if (progressIds.Any())
            {
                result.AllProgress = await _context.UserProgress
                    .Include(up => up.Simulation)
                    .ThenInclude(s => s!.Specialty)
                    .Include(up => up.User)
                    .Where(up => progressIds.Contains(up.Id))
                    .OrderByDescending(up => up.StartedAt)
                    .ToListAsync();
            }

            if (feedbackIds.Any())
            {
                result.Feedbacks = await _context.Feedbacks
                    .Where(f => feedbackIds.Contains(f.Id))
                    .OrderByDescending(f => f.CreatedAt)
                    .ToListAsync();
            }

            // Set selected student if filtering
            if (!string.IsNullOrEmpty(studentId))
            {
                result.SelectedStudent = result.Students.FirstOrDefault(s => s.ApplicationUserId == studentId);
            }
        }
        catch
        {
            await _context.Database.CloseConnectionAsync();
            throw;
        }

        return result;
    }

    public async Task<bool> GiveFeedbackAsync(int userProgressId, string instructorId, string feedbackText)
    {
        var userProgress = await _context.UserProgress
            .FirstOrDefaultAsync(up => up.Id == userProgressId);

        if (userProgress == null)
        {
            return false;
        }

        // Check if feedback already exists
        var existingFeedback = await _context.Feedbacks
            .FirstOrDefaultAsync(f => f.UserProgressId == userProgressId);

        if (existingFeedback != null)
        {
            // Update existing feedback
            existingFeedback.FeedbackText = feedbackText;
            existingFeedback.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            // Create new feedback
            var feedback = new Feedback
            {
                UserProgressId = userProgressId,
                SimulationId = userProgress.SimulationId,
                StudentId = userProgress.UserId,
                InstructorId = instructorId,
                FeedbackText = feedbackText,
                CreatedAt = DateTime.UtcNow
            };
            _context.Feedbacks.Add(feedback);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<AttemptDetailDto?> GetAttemptDetailAsync(int attemptId, string userId)
    {
        var attempt = await _context.UserProgress
            .Include(up => up.Simulation)
            .ThenInclude(s => s!.Specialty)
            .Include(up => up.User)
            .FirstOrDefaultAsync(up => up.Id == attemptId && up.UserId == userId);

        if (attempt == null)
        {
            return null;
        }

        // Get feedback for this attempt
        var feedback = await _context.Feedbacks
            .Include(f => f.Instructor)
            .FirstOrDefaultAsync(f => f.UserProgressId == attemptId);

        // Get instructor details if feedback exists
        Instructor? instructor = null;
        if (feedback != null)
        {
            instructor = await _context.Instructors
                .Include(i => i.Specialization)
                .FirstOrDefaultAsync(i => i.ApplicationUserId == feedback.InstructorId);
        }

        // Get all attempts for this simulation by this user for comparison
        var allAttemptsForSimulation = await _context.UserProgress
            .Where(up => up.UserId == userId && up.SimulationId == attempt.SimulationId)
            .OrderByDescending(up => up.StartedAt)
            .ToListAsync();

        // Calculate metrics
        var avgScoreForSimulation = allAttemptsForSimulation.Any()
            ? allAttemptsForSimulation.Average(up => (up.Score / (double)up.MaxScore) * 100)
            : 0;

        return new AttemptDetailDto
        {
            Attempt = attempt,
            Feedback = feedback,
            Instructor = instructor,
            AllAttempts = allAttemptsForSimulation,
            AverageScoreForSimulation = avgScoreForSimulation
        };
    }
}
