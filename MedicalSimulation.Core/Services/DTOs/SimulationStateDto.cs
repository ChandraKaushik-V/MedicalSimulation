namespace MedicalSimulation.Core.Services.DTOs;

public class SimulationStateDto
{
    public int Id { get; set; }
    public int StateNumber { get; set; }
    public string StateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public string? QuestionText { get; set; }
    public string? AnswerOptionsJson { get; set; }
    public int? CorrectAnswerIndex { get; set; }
    public string? HotspotDataJson { get; set; }
    public string? LayersJson { get; set; }
}
