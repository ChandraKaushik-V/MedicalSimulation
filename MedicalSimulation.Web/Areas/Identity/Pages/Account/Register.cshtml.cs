using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using MedicalSimulation.Core.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MedicalSimulation.Web.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RegisterModel> _logger;
    private readonly ApplicationDbContext _context;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = default!;
    
    public List<SelectListItem> Specializations { get; set; } = new();

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;

        [Required]
        [LettersOnly(ErrorMessage = "First name can only contain letters and spaces")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = default!;

        [Required]
        [LettersOnly(ErrorMessage = "Last name can only contain letters and spaces")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = default!;

        [Required]
        [PhoneNumberValidation(ErrorMessage = "Phone number must be exactly 10 digits")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = default!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = default!;

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = "Student";

        // Student-specific fields
        [Display(Name = "Student ID")]
        public string? StudentId { get; set; }

        [Display(Name = "Year Level")]
        [Range(1, 5, ErrorMessage = "Year level must be between 1 and 5")]
        public int? YearLevel { get; set; }

        // Instructor-specific fields
        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }

        [Display(Name = "Specialization")]
        public int? SpecializationId { get; set; }
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
        // Load specializations for dropdown
        Specializations = await _context.InstructorSpecializations
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
        // Reload specializations for dropdown in case of validation error
        Specializations = await _context.InstructorSpecializations
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
            .ToListAsync();
        
        // Check phone number uniqueness
        var phoneExists = await _context.Students.AnyAsync(s => s.PhoneNumber == Input.PhoneNumber) ||
                          await _context.Instructors.AnyAsync(i => i.PhoneNumber == Input.PhoneNumber);
        
        if (phoneExists)
        {
            ModelState.AddModelError("Input.PhoneNumber", "This phone number is already registered.");
        }
        
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser 
            { 
                UserName = Input.Email, 
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Assign Role
                await _userManager.AddToRoleAsync(user, Input.Role);

                // Create Student or Instructor record
                if (Input.Role == "Student")
                {
                    var student = new Student
                    {
                        ApplicationUserId = user.Id,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        StudentId = Input.StudentId ?? "",
                        YearLevel = Input.YearLevel ?? 1
                    };
                    _context.Students.Add(student);
                }
                else if (Input.Role == "Instructor")
                {
                    var instructor = new Instructor
                    {
                        ApplicationUserId = user.Id,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        EmployeeId = Input.EmployeeId ?? "",
                        SpecializationId = Input.SpecializationId ?? 1
                    };
                    _context.Instructors.Add(instructor);
                }

                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}
