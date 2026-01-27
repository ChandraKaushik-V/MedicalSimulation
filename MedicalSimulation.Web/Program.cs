using MedicalSimulation.Core.Data;
using MedicalSimulation.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=MedicalSimulationDb;Trusted_Connection=True;MultipleActiveResultSets=true";

// --- FIXED SECTION START ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("MedicalSimulation.Core"))
           .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));
// --- FIXED SECTION END ---

// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register Service Layer
builder.Services.AddScoped<MedicalSimulation.Core.Services.Interfaces.IDashboardService, MedicalSimulation.Core.Services.Implementations.DashboardService>();
builder.Services.AddScoped<MedicalSimulation.Core.Services.Interfaces.ISimulationService, MedicalSimulation.Core.Services.Implementations.SimulationService>();
builder.Services.AddScoped<MedicalSimulation.Core.Services.Interfaces.ISpecialtyService, MedicalSimulation.Core.Services.Implementations.SpecialtyService>();
builder.Services.AddScoped<MedicalSimulation.Core.Services.Interfaces.IProfileService, MedicalSimulation.Core.Services.Implementations.ProfileService>();


// Add session support for simulation state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // FIX DATABASE SCHEMA FIRST - Before DbInitializer runs
    
    // Note: ValidInstructorEmployeeIds table creation and Instructors table fixes 
    // should be handled by running the db_fix.sql script manually.
    
    // Drop unused UserProgress columns if they exist
    await context.Database.ExecuteSqlRawAsync(@"
        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'UserProgress') AND name = 'VitalSignsHistoryJson')
        BEGIN
            ALTER TABLE UserProgress DROP COLUMN VitalSignsHistoryJson;
        END
        
        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'UserProgress') AND name = 'DetailedStepDataJson')
        BEGIN
            ALTER TABLE UserProgress DROP COLUMN DetailedStepDataJson;
        END
        
        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'UserProgress') AND name = 'ClinicalErrorsJson')
        BEGIN
            ALTER TABLE UserProgress DROP COLUMN ClinicalErrorsJson;
        END
        
        IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'UserProgress') AND name = 'PerformanceMetricsJson')
        BEGIN
            ALTER TABLE UserProgress DROP COLUMN PerformanceMetricsJson;
        END
    ");
    
    // Update specialties to new structure
    await context.Database.ExecuteSqlRawAsync(@"
        UPDATE Specialties 
        SET Name = 'Dermatology', 
            Description = 'Skin and related conditions, including surgical procedures',
            IconClass = 'fa-hand-holding-medical',
            Color = '#10b981',
            DisplayOrder = 1,
            IsActive = 1
        WHERE Id = 1
    ");
    
    // Insert or update Neurology
    await context.Database.ExecuteSqlRawAsync(@"
        IF NOT EXISTS (SELECT 1 FROM Specialties WHERE Id = 2)
        BEGIN
            SET IDENTITY_INSERT Specialties ON;
            INSERT INTO Specialties (Id, Name, Description, IconClass, Color, DisplayOrder, IsActive)
            VALUES (2, 'Neurology', 'Brain, spinal cord, and nervous system procedures', 'fa-brain', '#8b5cf6', 2, 1);
            SET IDENTITY_INSERT Specialties OFF;
        END
        ELSE
        BEGIN
            UPDATE Specialties 
            SET Name = 'Neurology', 
                Description = 'Brain, spinal cord, and nervous system procedures',
                IconClass = 'fa-brain',
                Color = '#8b5cf6',
                DisplayOrder = 2,
                IsActive = 1
            WHERE Id = 2;
        END
    ");
    
    // Insert or update Cardiology
    await context.Database.ExecuteSqlRawAsync(@"
        IF NOT EXISTS (SELECT 1 FROM Specialties WHERE Id = 3)
        BEGIN
            SET IDENTITY_INSERT Specialties ON;
            INSERT INTO Specialties (Id, Name, Description, IconClass, Color, DisplayOrder, IsActive)
            VALUES (3, 'Cardiology', 'Heart and cardiovascular system procedures', 'fa-heart-pulse', '#ef4444', 3, 1);
            SET IDENTITY_INSERT Specialties OFF;
        END
        ELSE
        BEGIN
            UPDATE Specialties 
            SET Name = 'Cardiology', 
                Description = 'Heart and cardiovascular system procedures',
                IconClass = 'fa-heart-pulse',
                Color = '#ef4444',
                DisplayOrder = 3,
                IsActive = 1
            WHERE Id = 3;
        END
    ");
    
    // Update simulation mappings
    await context.Database.ExecuteSqlRawAsync(@"
        UPDATE Simulations SET SpecialtyId = 1 WHERE Id = 1;
        UPDATE Simulations SET SpecialtyId = 2 WHERE Id = 2;
    ");
    
    // NOW run DbInitializer after schema is fixed
    await DbInitializer.Initialize(services);
}

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
