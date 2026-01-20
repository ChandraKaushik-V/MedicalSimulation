using Microsoft.AspNetCore.Identity;

namespace MedicalSimulation.Core.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<UserProgress> UserProgress { get; set; } = new List<UserProgress>();
}
