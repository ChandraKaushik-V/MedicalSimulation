namespace MedicalSimulation.Core.Models;

public class Simulation
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int SpecialtyId { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public int EstimatedMinutes { get; set; }
    public int TotalStates { get; set; } // Number of states in this simulation
    public string? ThumbnailUrl { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Specialty? Specialty { get; set; }
    public virtual ICollection<UserProgress> UserProgress { get; set; } = new List<UserProgress>();
    public virtual ICollection<SurgeryState> States { get; set; } = new List<SurgeryState>();
}

public enum DifficultyLevel
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}
