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
    public required string Specialization { get; set; }

    // Navigation property
    public virtual ApplicationUser? ApplicationUser { get; set; }
}
