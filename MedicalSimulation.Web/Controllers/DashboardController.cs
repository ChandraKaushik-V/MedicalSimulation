using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using System.Security.Claims;

namespace MedicalSimulation.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userProgress = await _context.UserProgress
            .Include(up => up.Simulation)
            .ThenInclude(s => s!.Specialty)
            .Where(up => up.UserId == userId)
            .OrderByDescending(up => up.StartedAt)
            .ToListAsync();

        // Get feedback for user's attempts
        var progressIds = userProgress.Select(up => up.Id).ToList();
        var feedbacks = await _context.Feedbacks
            .Include(f => f.Instructor)
            .Where(f => progressIds.Contains(f.UserProgressId))
            .ToListAsync();

        // Get instructor details with specializations for feedback
        var instructorIds = feedbacks.Select(f => f.InstructorId).Distinct().ToList();
        var instructors = await _context.Instructors
            .Include(i => i.Specialization)
            .Where(i => instructorIds.Contains(i.ApplicationUserId))
            .ToListAsync();

        var completedSimulations = userProgress
            .Where(up => up.IsCompleted)
            .GroupBy(up => up.SimulationId)
            .Count();

        var totalAttempts = userProgress.Count;
        // Convert score from base 120 to base 100
        var averageScore = userProgress.Any() 
            ? userProgress.Average(up => (up.Score / (double)up.MaxScore) * 100) 
            : 0;

        ViewBag.CompletedSimulations = completedSimulations;
        ViewBag.TotalAttempts = totalAttempts;
        ViewBag.AverageScore = averageScore;
        ViewBag.RecentProgress = userProgress.Take(10).ToList();
        ViewBag.Feedbacks = feedbacks;
        ViewBag.Instructors = instructors;

        return View();
    }

    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> InstructorIndex(string? studentId = null)
    {
        var allProgress = await _context.UserProgress
            .Include(up => up.Simulation)
            .Include(up => up.User)
            .OrderByDescending(up => up.StartedAt)
            .ToListAsync();

        // Get student details for each attempt
        var userIds = allProgress.Select(up => up.UserId).Distinct().ToList();
        var students = await _context.Students
            .Where(s => userIds.Contains(s.ApplicationUserId))
            .ToListAsync();

        // Get existing feedbacks
        var progressIds = allProgress.Select(up => up.Id).ToList();
        var feedbacks = await _context.Feedbacks
            .Where(f => progressIds.Contains(f.UserProgressId))
            .ToListAsync();

        // If a specific student is selected, filter the progress
        if (!string.IsNullOrEmpty(studentId))
        {
            allProgress = allProgress.Where(up => up.UserId == studentId).ToList();
            ViewBag.SelectedStudentId = studentId;
            ViewBag.SelectedStudent = students.FirstOrDefault(s => s.ApplicationUserId == studentId);
        }
        else
        {
            // Calculate student summary statistics for the student list view
            var studentSummaries = students.Select(student => new
            {
                Student = student,
                AttemptCount = allProgress.Count(p => p.UserId == student.ApplicationUserId),
                CompletedCount = allProgress.Count(p => p.UserId == student.ApplicationUserId && p.IsCompleted),
                AverageScore = allProgress.Where(p => p.UserId == student.ApplicationUserId && p.IsCompleted).Any()
                    ? allProgress.Where(p => p.UserId == student.ApplicationUserId && p.IsCompleted).Average(p => p.Score)
                    : 0,
                PendingFeedback = allProgress.Count(p => p.UserId == student.ApplicationUserId && !feedbacks.Any(f => f.UserProgressId == p.Id))
            }).OrderByDescending(s => s.AttemptCount).ToList();

            ViewBag.StudentSummaries = studentSummaries;
        }

        ViewBag.Students = students;
        ViewBag.Feedbacks = feedbacks;

        return View(allProgress);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPost]
    public async Task<IActionResult> GiveFeedback(int userProgressId, string feedbackText)
    {
        var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var userProgress = await _context.UserProgress
            .FirstOrDefaultAsync(up => up.Id == userProgressId);

        if (userProgress == null)
        {
            return NotFound();
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
                InstructorId = instructorId!,
                FeedbackText = feedbackText,
                CreatedAt = DateTime.UtcNow
            };
            _context.Feedbacks.Add(feedback);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(InstructorIndex));
    }

    public async Task<IActionResult> AttemptDetail(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var attempt = await _context.UserProgress
            .Include(up => up.Simulation)
            .ThenInclude(s => s!.Specialty)
            .Include(up => up.User)
            .FirstOrDefaultAsync(up => up.Id == id && up.UserId == userId);

        if (attempt == null)
        {
            return NotFound();
        }

        // Get feedback for this attempt
        var feedback = await _context.Feedbacks
            .Include(f => f.Instructor)
            .FirstOrDefaultAsync(f => f.UserProgressId == id);

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

        ViewBag.Feedback = feedback;
        ViewBag.Instructor = instructor;
        ViewBag.AllAttempts = allAttemptsForSimulation;
        ViewBag.AverageScoreForSimulation = avgScoreForSimulation;

        return View(attempt);
    }
}
