namespace MedicalSimulation.Core.Models;

public class Feedback
{
    public int Id { get; set; }
    
    // Link to the specific student attempt
    public int UserProgressId { get; set; }
    
    // Direct references for easier querying
    public int SimulationId { get; set; }
    public required string StudentId { get; set; }
    public required string InstructorId { get; set; }
    
    // Feedback content
    public required string FeedbackText { get; set; }
    
    // Timestamp
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual UserProgress? UserProgress { get; set; }
    public virtual Simulation? Simulation { get; set; }
    public virtual ApplicationUser? Student { get; set; }
    public virtual ApplicationUser? Instructor { get; set; }
}
