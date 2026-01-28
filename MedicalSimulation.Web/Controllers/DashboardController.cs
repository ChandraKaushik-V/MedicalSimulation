using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalSimulation.Core.Services.Interfaces;
using System.Security.Claims;

namespace MedicalSimulation.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var dashboardData = await _dashboardService.GetStudentDashboardAsync(userId);

        ViewBag.CompletedSimulations = dashboardData.CompletedSimulations;
        ViewBag.TotalAttempts = dashboardData.TotalAttempts;
        ViewBag.AverageScore = dashboardData.AverageScore;
        ViewBag.RecentProgress = dashboardData.RecentProgress;
        ViewBag.Feedbacks = dashboardData.Feedbacks;
        ViewBag.Instructors = dashboardData.Instructors;

        return View();
    }

    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> InstructorIndex(string? studentId = null)
    {
        var instructorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (instructorUserId == null)
        {
            return Unauthorized();
        }

        var dashboardData = await _dashboardService.GetInstructorDashboardAsync(instructorUserId, studentId);

        if (dashboardData.AllProgress.Count == 0 && dashboardData.Students.Count == 0)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Students = dashboardData.Students;
        ViewBag.Feedbacks = dashboardData.Feedbacks;
        
        if (!string.IsNullOrEmpty(studentId))
        {
            ViewBag.SelectedStudentId = dashboardData.SelectedStudentId;
            ViewBag.SelectedStudent = dashboardData.SelectedStudent;
        }
        else
        {
            ViewBag.StudentSummaries = dashboardData.StudentSummaries;
        }

        return View(dashboardData.AllProgress);
    }

    [Authorize(Roles = "Instructor")]
    [HttpPost]
    public async Task<IActionResult> GiveFeedback(int userProgressId, string feedbackText)
    {
        var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (instructorId == null)
        {
            return Unauthorized();
        }

        var success = await _dashboardService.GiveFeedbackAsync(userProgressId, instructorId, feedbackText);

        if (!success)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Feedback sent successfully!";
        return RedirectToAction(nameof(InstructorIndex));
    }

    public async Task<IActionResult> AttemptDetail(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var attemptDetail = await _dashboardService.GetAttemptDetailAsync(id, userId);

        if (attemptDetail == null)
        {
            return NotFound();
        }

        ViewBag.Feedback = attemptDetail.Feedback;
        ViewBag.Instructor = attemptDetail.Instructor;
        ViewBag.AllAttempts = attemptDetail.AllAttempts;
        ViewBag.AverageScoreForSimulation = attemptDetail.AverageScoreForSimulation;

        return View(attemptDetail.Attempt);
    }
}
