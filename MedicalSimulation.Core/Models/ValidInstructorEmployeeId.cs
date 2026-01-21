namespace MedicalSimulation.Core.Models;

public class ValidInstructorEmployeeId
{
    public int Id { get; set; }
    public required string EmployeeId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
