namespace MedicalSimulation.Core.Models;

public class Instructor
{
    public int Id { get; set; }
    public required string ApplicationUserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string EmployeeId { get; set; }
    public int SpecializationId { get; set; }

    // Navigation properties
    public virtual ApplicationUser? ApplicationUser { get; set; }
    public virtual InstructorSpecialization? Specialization { get; set; }
}
