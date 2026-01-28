namespace MedicalSimulation.Core.Services.DTOs;

public class ValidationResultDto
{
    public bool IsCorrect { get; set; }
    public int CorrectAnswerIndex { get; set; }
    public int PointsEarned { get; set; }
}
