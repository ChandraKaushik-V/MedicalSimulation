namespace MedicalSimulation.Core.Models;

public class InstructorSpecialization
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    // Navigation property
    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
