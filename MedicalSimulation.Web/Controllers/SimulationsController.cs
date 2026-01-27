using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalSimulation.Core.Services.DTOs;
using MedicalSimulation.Core.Services.Interfaces;
using System.Security.Claims;

namespace MedicalSimulation.Web.Controllers;

[Authorize]
public class SimulationsController : Controller
{
    private readonly ISimulationService _simulationService;

    public SimulationsController(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var simulationDetail = await _simulationService.GetSimulationDetailsAsync(id, userId);

        if (simulationDetail == null)
        {
            return NotFound();
        }

        ViewBag.UserProgress = simulationDetail.UserProgress;
        ViewBag.BestScore = simulationDetail.BestScore;
        ViewBag.Attempts = simulationDetail.Attempts;

        return View(simulationDetail.Simulation);
    }

    public async Task<IActionResult> Start(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var (simulation, progressId) = await _simulationService.StartSimulationAsync(id, userId);

        if (simulation == null)
        {
            return NotFound();
        }

        // Store progress ID in session
        HttpContext.Session.SetInt32("CurrentProgressId", progressId);

        return View(simulation);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitProgress([FromBody] ProgressSubmissionDto submission)
    {
        var progressId = HttpContext.Session.GetInt32("CurrentProgressId");

        if (progressId == null)
        {
            return BadRequest("No active simulation session");
        }

        var success = await _simulationService.SubmitProgressAsync(progressId.Value, submission);

        if (!success)
        {
            return NotFound();
        }

        return Ok(new { success = true, progressId = progressId.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetStates(int simulationId)
    {
        var states = await _simulationService.GetStatesAsync(simulationId);
        return Json(states);
    }

    [HttpPost]
    public async Task<IActionResult> ValidateAnswer([FromBody] AnswerSubmissionDto submission)
    {
        var isCorrect = await _simulationService.ValidateAnswerAsync(submission);
        return Ok(new { isCorrect });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateScore([FromBody] ScoreUpdateDto scoreUpdate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var success = await _simulationService.UpdateScoreAsync(scoreUpdate, userId);

        if (!success)
        {
            return NotFound();
        }

        return Ok(new { success = true });
    }

    public async Task<IActionResult> Results(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var progress = await _simulationService.GetResultsAsync(id, userId);

        if (progress == null)
        {
            return NotFound();
        }

        return View(progress);
    }
}
