using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using System.Security.Claims;
using System.Text.Json;

namespace MedicalSimulation.Web.Controllers;

[Authorize]
public class SimulationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SimulationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Details(int id)
    {
        var simulation = await _context.Simulations
            .Include(s => s.Specialty)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (simulation == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userProgress = await _context.UserProgress
            .Where(up => up.UserId == userId && up.SimulationId == id)
            .OrderByDescending(up => up.StartedAt)
            .ToListAsync();

        ViewBag.UserProgress = userProgress;
        ViewBag.BestScore = userProgress.Any() ? userProgress.Max(up => up.Score) : 0;
        ViewBag.Attempts = userProgress.Count;

        return View(simulation);
    }

    public async Task<IActionResult> Start(int id)
    {
        var simulation = await _context.Simulations
            .Include(s => s.Specialty)
            .Include(s => s.States.OrderBy(st => st.StateNumber))
            .FirstOrDefaultAsync(s => s.Id == id);

        if (simulation == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        // Create new progress entry
        var attemptNumber = await _context.UserProgress
            .Where(up => up.UserId == userId && up.SimulationId == id)
            .CountAsync() + 1;

        var progress = new UserProgress
        {
            UserId = userId!,
            SimulationId = id,
            Score = 0,
            MaxScore = 120,
            IsCompleted = false,
            AttemptNumber = attemptNumber,
            TimeSpent = TimeSpan.Zero,
            StartedAt = DateTime.UtcNow
        };

        _context.UserProgress.Add(progress);
        await _context.SaveChangesAsync();

        // Store progress ID in session
        HttpContext.Session.SetInt32("CurrentProgressId", progress.Id);

        return View(simulation);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitProgress([FromBody] ProgressSubmission submission)
    {
        var progressId = HttpContext.Session.GetInt32("CurrentProgressId");
        
        if (progressId == null)
        {
            return BadRequest("No active simulation session");
        }

        var progress = await _context.UserProgress.FindAsync(progressId.Value);
        
        if (progress == null)
        {
            return NotFound();
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

        return Ok(new { success = true, progressId = progress.Id });
    }

    [HttpGet]
    public async Task<IActionResult> GetStates(int simulationId)
    {
        var states = await _context.SurgeryStates
            .Where(s => s.SimulationId == simulationId)
            .OrderBy(s => s.StateNumber)
            .Select(s => new
            {
                s.Id,
                s.StateNumber,
                s.StateName,
                s.Description,
                s.VideoUrl,
                s.InteractionType,
                s.QuestionText,
                s.AnswerOptionsJson,
                s.CorrectAnswerIndex,
                s.HotspotDataJson,
                s.LayersJson // Keep for backward compatibility
            })
            .ToListAsync();

        return Json(states);
    }

    [HttpPost]
    public async Task<IActionResult> ValidateAnswer([FromBody] AnswerSubmission submission)
    {
        var state = await _context.SurgeryStates.FindAsync(submission.StateId);
        
        if (state == null)
        {
            return NotFound();
        }

        bool isCorrect = state.CorrectAnswerIndex == submission.AnswerIndex;
        
        return Ok(new { isCorrect });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateScore([FromBody] ScoreUpdate scoreUpdate)
    {
        var progress = await _context.UserProgress.FindAsync(scoreUpdate.ProgressId);
        
        if (progress == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (progress.UserId != userId)
        {
            return Forbid();
        }

        progress.Score = scoreUpdate.Score;
        progress.IsCompleted = true;
        progress.CompletedAt = DateTime.UtcNow;
        progress.TimeSpent = DateTime.UtcNow - progress.StartedAt;

        await _context.SaveChangesAsync();

        return Ok(new { success = true });
    }

    public async Task<IActionResult> Results(int id)
    {
        var progress = await _context.UserProgress
            .Include(up => up.Simulation)
            .ThenInclude(s => s!.Specialty)
            .FirstOrDefaultAsync(up => up.Id == id);

        if (progress == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (progress.UserId != userId)
        {
            return Forbid();
        }

        return View(progress);
    }
}

public class ProgressSubmission
{
    public int Score { get; set; }
    public bool IsCompleted { get; set; }
    public int TimeSpentSeconds { get; set; }
    public Dictionary<string, object>? Feedback { get; set; }
}

public class AnswerSubmission
{
    public int StateId { get; set; }
    public int AnswerIndex { get; set; }
}

public class ScoreUpdate
{
    public int ProgressId { get; set; }
    public int Score { get; set; }
}

