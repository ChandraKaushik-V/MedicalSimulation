using MedicalSimulation.Core.Models;
using MedicalSimulation.Core.Services.DTOs;

namespace MedicalSimulation.Core.Services.Interfaces;

public interface ISimulationService
{
    Task<SimulationDetailDto?> GetSimulationDetailsAsync(int simulationId, string userId);
    Task<(Simulation? simulation, int progressId)> StartSimulationAsync(int simulationId, string userId);
    Task<bool> SubmitProgressAsync(int progressId, ProgressSubmissionDto submission);
    Task<List<SimulationStateDto>> GetStatesAsync(int simulationId);
    Task<ValidationResultDto> ValidateAnswerAsync(int progressId, int stateId, int answerIndex, int timeSpentSeconds);
    Task<bool> UpdateScoreAsync(ScoreUpdateDto scoreUpdate, string userId);
    Task<UserProgress?> GetResultsAsync(int progressId, string userId);
    Task<TimeSpan?> CompleteSimulationAsync(int progressId);
}
