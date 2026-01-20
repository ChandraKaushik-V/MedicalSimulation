namespace MedicalSimulation.Core.Models;

public class SurgeryState
{
    public int Id { get; set; }
    public int SimulationId { get; set; }
    public int StateNumber { get; set; }
    public required string StateName { get; set; }
    public required string Description { get; set; }
    
    // Video-based approach
    public string? VideoUrl { get; set; } // Path to pre-rendered video file
    
    // Interaction type: "click-hotspot", "mcq", "auto-play"
    public string? InteractionType { get; set; }
    
    // For click-hotspot: JSON with {x, y, radius, correctFeedback, incorrectFeedback}
    public string? HotspotDataJson { get; set; }
    
    // MCQ support
    public string? QuestionText { get; set; } // MCQ question for this state
    public string? AnswerOptionsJson { get; set; } // JSON array of 4 answer choices
    public int? CorrectAnswerIndex { get; set; } // Index of correct answer (0-3)
    
    // Legacy 2.5D approach (deprecated but kept for backward compatibility)
    public string? LayersJson { get; set; } // JSON array of layer image URLs with z-depths
    
    public virtual Simulation? Simulation { get; set; }
}
