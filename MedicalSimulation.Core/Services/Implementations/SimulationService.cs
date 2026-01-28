using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using MedicalSimulation.Core.Services.DTOs;
using MedicalSimulation.Core.Services.Interfaces;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MedicalSimulation.Core.Services.Implementations;

public class SimulationService : ISimulationService
{
    private readonly ApplicationDbContext _context;

    public SimulationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SimulationDetailDto?> GetSimulationDetailsAsync(int simulationId, string userId)
    {
        var simulation = await _context.Simulations
            .Include(s => s.Specialty)
            .FirstOrDefaultAsync(s => s.Id == simulationId);

        if (simulation == null)
        {
            return null;
        }

        var userProgress = await _context.UserProgress
            .Where(up => up.UserId == userId && up.SimulationId == simulationId)
            .OrderByDescending(up => up.StartedAt)
            .ToListAsync();

        return new SimulationDetailDto
        {
            Simulation = simulation,
            UserProgress = userProgress,
            BestScore = userProgress.Any() ? userProgress.Max(up => up.Score) : 0,
            Attempts = userProgress.Count
        };
    }

    public async Task<(Simulation? simulation, int progressId)> StartSimulationAsync(int simulationId, string userId)
    {
        // First get simulation details
        var simulation = await _context.Simulations
            .Include(s => s.Specialty)
            .Include(s => s.States.OrderBy(st => st.StateNumber))
            .FirstOrDefaultAsync(s => s.Id == simulationId);

        if (simulation == null)
        {
            return (null, 0);
        }

        // Call stored procedure to create progress entry
        int progressId = 0;
        int attemptNumber = 0;

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@SimulationId", simulationId),
                new SqlParameter("@MaxScore", 120)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_StartSimulationAttempt";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                progressId = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("ProgressId")));
                attemptNumber = reader.GetInt32(reader.GetOrdinal("AttemptNumber"));
            }
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }

        return (simulation, progressId);
    }

    public async Task<bool> SubmitProgressAsync(int progressId, ProgressSubmissionDto submission)
    {
        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ProgressId", progressId),
                new SqlParameter("@CurrentScore", submission.Score),
                new SqlParameter("@TimeSpent", TimeSpan.FromSeconds(submission.TimeSpentSeconds)),
                new SqlParameter("@FeedbackJson", JsonSerializer.Serialize(submission.Feedback) ?? (object)DBNull.Value)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_UpdateSimulationProgress";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetInt32(reader.GetOrdinal("Success")) == 1;
            }

            return false;
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }
    }

    public async Task<List<SimulationStateDto>> GetStatesAsync(int simulationId)
    {
        var states = await _context.SurgeryStates
            .Where(s => s.SimulationId == simulationId)
            .OrderBy(s => s.StateNumber)
            .Select(s => new SimulationStateDto
            {
                Id = s.Id,
                StateNumber = s.StateNumber,
                StateName = s.StateName,
                Description = s.Description,
                VideoUrl = s.VideoUrl,
                InteractionType = s.InteractionType,
                QuestionText = s.QuestionText,
                AnswerOptionsJson = s.AnswerOptionsJson,
                CorrectAnswerIndex = s.CorrectAnswerIndex,
                HotspotDataJson = s.HotspotDataJson,
                LayersJson = s.LayersJson
            })
            .ToListAsync();

        return states;
    }

    public async Task<ValidationResultDto> ValidateAnswerAsync(int progressId, int stateId, int answerIndex, int timeSpentSeconds)
    {
        var result = new ValidationResultDto();

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ProgressId", progressId),
                new SqlParameter("@StateId", stateId),
                new SqlParameter("@AnswerIndex", answerIndex),
                new SqlParameter("@TimeSpentSeconds", timeSpentSeconds)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_ValidateSimulationAnswer";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result.IsCorrect = reader.GetBoolean(reader.GetOrdinal("IsCorrect"));
                result.CorrectAnswerIndex = reader.GetInt32(reader.GetOrdinal("CorrectAnswerIndex"));
                result.PointsEarned = reader.GetInt32(reader.GetOrdinal("PointsEarned"));
            }
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }

        return result;
    }

    public async Task<bool> UpdateScoreAsync(ScoreUpdateDto scoreUpdate, string userId)
    {
        var progress = await _context.UserProgress.FindAsync(scoreUpdate.ProgressId);

        if (progress == null || progress.UserId != userId)
        {
            return false;
        }

        progress.Score = scoreUpdate.Score;
        progress.IsCompleted = true;
        progress.CompletedAt = DateTime.UtcNow;
        progress.TimeSpent = DateTime.UtcNow - progress.StartedAt;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UserProgress?> GetResultsAsync(int progressId, string userId)
    {
        var progress = await _context.UserProgress
            .Include(up => up.Simulation)
            .ThenInclude(s => s!.Specialty)
            .FirstOrDefaultAsync(up => up.Id == progressId);

        if (progress == null || progress.UserId != userId)
        {
            return null;
        }

        return progress;
    }

    public async Task<TimeSpan?> CompleteSimulationAsync(int progressId)
    {
        TimeSpan? timeSpent = null;

        try
        {
            var parameters = new[]
            {
                new SqlParameter("@ProgressId", progressId)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "sp_CompleteSimulationAttempt";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var success = reader.GetInt32(reader.GetOrdinal("Success"));
                if (success == 1)
                {
                    timeSpent = (TimeSpan)reader.GetValue(reader.GetOrdinal("TimeSpent"));
                }
            }
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }

        return timeSpent;
    }
}
