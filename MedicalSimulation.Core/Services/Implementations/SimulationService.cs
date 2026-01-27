using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using MedicalSimulation.Core.Services.DTOs;
using MedicalSimulation.Core.Services.Interfaces;
using System.Text.Json;

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
        var simulation = await _context.Simulations
            .Include(s => s.Specialty)
            .Include(s => s.States.OrderBy(st => st.StateNumber))
            .FirstOrDefaultAsync(s => s.Id == simulationId);

        if (simulation == null)
        {
            return (null, 0);
        }

        // Create new progress entry
        var attemptNumber = await _context.UserProgress
            .Where(up => up.UserId == userId && up.SimulationId == simulationId)
            .CountAsync() + 1;

        var progress = new UserProgress
        {
            UserId = userId,
            SimulationId = simulationId,
            Score = 0,
            MaxScore = 120,
            IsCompleted = false,
            AttemptNumber = attemptNumber,
            TimeSpent = TimeSpan.Zero,
            StartedAt = DateTime.UtcNow
        };

        _context.UserProgress.Add(progress);
        await _context.SaveChangesAsync();

        return (simulation, progress.Id);
    }

    public async Task<bool> SubmitProgressAsync(int progressId, ProgressSubmissionDto submission)
    {
        var progress = await _context.UserProgress.FindAsync(progressId);

        if (progress == null)
        {
            return false;
        }

        progress.Score = submission.Score;
        progress.IsCompleted = submission.IsCompleted;
        progress.TimeSpent = TimeSpan.FromSeconds(submission.TimeSpentSeconds);
        progress.FeedbackJson = JsonSerializer.Serialize(submission.Feedback);

        if (submission.IsCompleted)
        {
            progress.CompletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
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

    public async Task<bool> ValidateAnswerAsync(AnswerSubmissionDto submission)
    {
        var state = await _context.SurgeryStates.FindAsync(submission.StateId);

        if (state == null)
        {
            return false;
        }

        return state.CorrectAnswerIndex == submission.AnswerIndex;
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
}
