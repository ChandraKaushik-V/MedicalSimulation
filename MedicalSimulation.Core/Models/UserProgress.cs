namespace MedicalSimulation.Core.Models;

public class UserProgress
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int SimulationId { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public bool IsCompleted { get; set; }
    public int AttemptNumber { get; set; }
    public TimeSpan TimeSpent { get; set; }
    public string? FeedbackJson { get; set; } // Stores detailed feedback/mistakes
    
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    // Navigation properties
    public virtual ApplicationUser? User { get; set; }
    public virtual Simulation? Simulation { get; set; }
}
