using MedicalSimulation.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalSimulation.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Simulation> Simulations { get; set; }
        public DbSet<SurgeryState> SurgeryStates { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<InstructorSpecialization> InstructorSpecializations { get; set; }
        public DbSet<ValidInstructorEmployeeId> ValidInstructorEmployeeIds { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Specialties
            builder.Entity<Specialty>().HasData(
                new Specialty 
                { 
                    Id = 1, 
                    Name = "Dermatology", 
                    Description = "Skin and related conditions, including surgical procedures",
                    IconClass = "fa-hand-holding-medical",
                    Color = "#10b981",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new Specialty 
                { 
                    Id = 2, 
                    Name = "Neurology", 
                    Description = "Brain, spinal cord, and nervous system procedures",
                    IconClass = "fa-brain",
                    Color = "#8b5cf6",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new Specialty 
                { 
                    Id = 3, 
                    Name = "Cardiology", 
                    Description = "Heart and cardiovascular system procedures",
                    IconClass = "fa-heart-pulse",
                    Color = "#ef4444",
                    DisplayOrder = 3,
                    IsActive = true
                }
            );

            // Seed InstructorSpecializations
            builder.Entity<InstructorSpecialization>().HasData(
                new InstructorSpecialization { Id = 1, Name = "Dermatology", Description = "Skin and related conditions" },
                new InstructorSpecialization { Id = 2, Name = "Neurosurgery", Description = "Brain and nervous system surgery" },
                new InstructorSpecialization { Id = 3, Name = "Cardiology", Description = "Heart and cardiovascular system" }
            );

            // Seed ValidInstructorEmployeeIds
            builder.Entity<ValidInstructorEmployeeId>().HasData(
                new ValidInstructorEmployeeId { Id = 1, EmployeeId = "EMP001", FirstName = "John", LastName = "Doe", IsActive = true },
                new ValidInstructorEmployeeId { Id = 2, EmployeeId = "EMP002", FirstName = "Jane", LastName = "Smith", IsActive = true },
                new ValidInstructorEmployeeId { Id = 3, EmployeeId = "EMP003", FirstName = "Michael", LastName = "Johnson", IsActive = true },
                new ValidInstructorEmployeeId { Id = 4, EmployeeId = "EMP004", FirstName = "Sarah", LastName = "Williams", IsActive = true },
                new ValidInstructorEmployeeId { Id = 5, EmployeeId = "EMP005", FirstName = "David", LastName = "Brown", IsActive = true }
            );

            // Seed Simulation
            builder.Entity<Simulation>().HasData(
                new Simulation
                {
                    Id = 1,
                    Title = "Subcuticular Running Suture",
                    Description = "Master the subcuticular running suture technique with this interactive video simulation.",
                    SpecialtyId = 1, // Dermatology
                    Difficulty = DifficultyLevel.Intermediate,
                    EstimatedMinutes = 15,
                    TotalStates = 6,
                    ThumbnailUrl = "/pictures/suture.png"
                },
                new Simulation
                {
                    Id = 2,
                    Title = "Posterior Neck Craniectomy",
                    Description = "Learn the critical steps and anatomical considerations in performing a posterior neck craniectomy.",
                    SpecialtyId = 2, // Neurology
                    Difficulty = DifficultyLevel.Advanced,
                    EstimatedMinutes = 10,
                    TotalStates = 6,
                    ThumbnailUrl = "/pictures/craniectomy.png"
                }
            );

            // Seed States (Single Video Flow with Timed Pauses)
            // Video pauses at: 0.922s, 6.423s, 12.13s, 51.449s, 60s
            // Questions 1, 2, 3: Click hotspot (Points 1, 2, 3)
            // Question 4: Click hotspot - "Where should the cut be made" (Point 4)
            // Question 5: MCQ - "What is the next correct step"
            // Question 6: Click hotspot - "Click the correct point to place the next intradermal bite" (Point 5)
            
            string videoPath = "/videos/simulations/subcuticular-running-suture.mp4";

            builder.Entity<SurgeryState>().HasData(
                // State 1: Pause at 0.922s - First Intradermal Bite Entry (Point 1)
                new SurgeryState
                {
                    Id = 1,
                    SimulationId = 1,
                    StateNumber = 1,
                    StateName = "First Intradermal Bite Entry",
                    Description = "Click the correct entry point for the first intradermal bite.",
                    VideoUrl = videoPath,
                    InteractionType = "click-hotspot",
                    QuestionText = "Click the correct entry point for the first intradermal bite.",
                    HotspotDataJson = "{\"pauseTime\":0.922,\"x\":55.21,\"y\":52.76,\"radius\":10}", 
                    LayersJson = "[]"
                },
                // State 2: Pause at 6.423s - Needle Exit Point (Point 2)
                new SurgeryState
                {
                    Id = 2,
                    SimulationId = 1,
                    StateNumber = 2,
                    StateName = "Needle Exit Point",
                    Description = "Click where the needle should exit on the opposite side.",
                    VideoUrl = videoPath,
                    InteractionType = "click-hotspot",
                    QuestionText = "Click where the needle should exit on the opposite side.",
                    HotspotDataJson = "{\"pauseTime\":6.423,\"x\":50,\"y\":72.76,\"radius\":10}",
                    LayersJson = "[]"
                },
                // State 3: Pause at 12.13s - Suture Loop Position (Point 3)
                new SurgeryState
                {
                    Id = 3,
                    SimulationId = 1,
                    StateNumber = 3,
                    StateName = "Suture Loop Position",
                    Description = "Click where the suture loop should rest before tightening.",
                    VideoUrl = videoPath,
                    InteractionType = "click-hotspot",
                    QuestionText = "Click where the suture loop should rest before tightening.",
                    HotspotDataJson = "{\"pauseTime\":12.13,\"x\":48.81,\"y\":63.01,\"radius\":10}",
                    LayersJson = "[]"
                },
                // State 4: Pause at 51.449s - Cut Placement (Point 4)
                new SurgeryState
                {
                    Id = 4,
                    SimulationId = 1,
                    StateNumber = 4,
                    StateName = "Cut Placement",
                    Description = "Where should the cut be made?",
                    VideoUrl = videoPath,
                    InteractionType = "click-hotspot",
                    QuestionText = "Where should the cut be made?",
                    HotspotDataJson = "{\"pauseTime\":51.449,\"x\":57.68,\"y\":27.57,\"radius\":10}",
                    LayersJson = "[]"
                },
                // State 5: Pause at 60s - MCQ (Next Step After Knot)
                new SurgeryState
                {
                    Id = 5,
                    SimulationId = 1,
                    StateNumber = 5,
                    StateName = "Next Step After Knot",
                    Description = "What is the next correct step after securing this knot in a running subcuticular suture?",
                    VideoUrl = videoPath,
                    InteractionType = "mcq",
                    QuestionText = "What is the next correct step after securing this knot in a running subcuticular suture?",
                    HotspotDataJson = "{\"pauseTime\":57.000}",
                    AnswerOptionsJson = "[\"Begin the next intradermal bite at equal spacing\",\"Cut the suture ends\",\"Tighten further to blanch the skin\",\"Place a superficial interrupted stitch\"]",
                    CorrectAnswerIndex = 0,
                    LayersJson = "[]"
                },
                // State 6: Pause at 60s (after MCQ) - Next Intradermal Bite (Point 5)
                new SurgeryState
                {
                    Id = 6,
                    SimulationId = 1,
                    StateNumber = 6,
                    StateName = "Next Intradermal Bite",
                    Description = "Click the correct point to place the next intradermal bite.",
                    VideoUrl = videoPath,
                    InteractionType = "click-hotspot",
                    QuestionText = "Click the correct point to place the next intradermal bite.",
                    HotspotDataJson = "{\"pauseTime\":60,\"x\":42.32,\"y\":58.94,\"radius\":10}",
                    LayersJson = "[]"
                },

                // ===== CRANIECTOMY SIMULATION STATES =====
                // State 1: Pause at 3.765s - Incision Line (Point 1)
                new SurgeryState
                {
                    Id = 7,
                    SimulationId = 2,
                    StateNumber = 1,
                    StateName = "Incision Line Identification",
                    Description = "Click on the correct incision line for a posterior neck craniectomy.",
                    VideoUrl = "/videos/simulations/craniectomy.mp4",
                    InteractionType = "click-hotspot",
                    QuestionText = "Click on the correct incision line for a posterior neck craniectomy.",
                    HotspotDataJson = "{\"pauseTime\":3.765,\"x\":50.46,\"y\":38.95,\"radius\":5}",
                    LayersJson = "[]"
                },
                // State 2: Pause at 16.000s - MCQ about structure identification
                new SurgeryState
                {
                    Id = 8,
                    SimulationId = 2,
                    StateNumber = 2,
                    StateName = "Structure Identification",
                    Description = "Identify the critical structure before proceeding.",
                    VideoUrl = "/videos/simulations/craniectomy.mp4",
                    InteractionType = "mcq",
                    QuestionText = "Which structure must be clearly identified before proceeding further in a posterior neck carcinectomy?",
                    HotspotDataJson = "{\"pauseTime\":16.000}",
                    AnswerOptionsJson = "[\"Skin and subcutaneous tissue\",\"Trapezius muscle\",\"Deep cervical fascia and underlying vital structures\",\"Vertebral spinous processes\"]",
                    CorrectAnswerIndex = 2,
                    LayersJson = "[]"
                },
                // State 3: Pause at 28.000s - MCQ about resection margin principles
                new SurgeryState
                {
                    Id = 9,
                    SimulationId = 2,
                    StateNumber = 3,
                    StateName = "Resection Margin Principles",
                    Description = "Understanding the correct principle for defining resection margins.",
                    VideoUrl = "/videos/simulations/craniectomy.mp4",
                    InteractionType = "mcq",
                    QuestionText = "What is the correct principle for defining the resection margin during a carcinectomy?",
                    HotspotDataJson = "{\"pauseTime\":28.000}",
                    AnswerOptionsJson = "[\"Excise only the visibly abnormal tissue\",\"Cut as close as possible to minimize tissue loss\",\"Include an adequate margin of healthy tissue around the lesion\",\"Delay margin planning until closure\"]",
                    CorrectAnswerIndex = 2,
                    LayersJson = "[]"
                },
                // State 4: Pause at 56.669s - Next incision point
                new SurgeryState
                {
                    Id = 10,
                    SimulationId = 2,
                    StateNumber = 4,
                    StateName = "Next Incision Point",
                    Description = "Point where the next incision is to be done.",
                    VideoUrl = "/videos/simulations/craniectomy.mp4",
                    InteractionType = "click-hotspot",
                    QuestionText = "Point where the next incision is to be done.",
                    HotspotDataJson = "{\"pauseTime\":56.669,\"x\":49.91,\"y\":40.25,\"radius\":5}",
                    LayersJson = "[]"
                },
                // State 5: Pause at 81.431s (01:21.431) - MCQ about structure protection
                new SurgeryState
                {
                    Id = 11,
                    SimulationId = 2,
                    StateNumber = 5,
                    StateName = "Structure Protection",
                    Description = "Identify the structure that must be actively protected during dissection.",
                    VideoUrl = "/videos/simulations/craniectomy.mp4",
                    InteractionType = "mcq",
                    QuestionText = "Which structure must be actively protected while performing this dissection?",
                    HotspotDataJson = "{\"pauseTime\":81.431}",
                    AnswerOptionsJson = "[\"Skin flap\",\"Paraspinal muscles\",\"Spinal cord and dura mater\",\"Trachea\"]",
                    CorrectAnswerIndex = 2,
                    LayersJson = "[]"
                },
                // State 6: Pause at 93.431s (01:33.431) - MCQ about next step
                new SurgeryState
                {
                    Id = 12,
                    SimulationId = 2,
                    StateNumber = 6,
                    StateName = "Post-Excision Management",
                    Description = "Determine the appropriate next step after excising the lesion.",
                    VideoUrl = "/videos/simulations/craniectomy.mp4",
                    InteractionType = "mcq",
                    QuestionText = "What is the next appropriate step after excising the lesion?",
                    HotspotDataJson = "{\"pauseTime\":93.431}",
                    AnswerOptionsJson = "[\"Immediate skin closure with sutures\",\"Irrigation and leave the wound open\",\"Apply a sterile pressure dressing / protective dressing\",\"Begin deep muscle reconstruction\"]",
                    CorrectAnswerIndex = 2,
                    LayersJson = "[]"
                }
            );

            // Configure Feedback relationships
            builder.Entity<Feedback>()
                .HasOne(f => f.Student)
                .WithMany()
                .HasForeignKey(f => f.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Feedback>()
                .HasOne(f => f.Instructor)
                .WithMany()
                .HasForeignKey(f => f.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Feedback>()
                .HasOne(f => f.UserProgress)
                .WithMany()
                .HasForeignKey(f => f.UserProgressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Feedback>()
                .HasOne(f => f.Simulation)
                .WithMany()
                .HasForeignKey(f => f.SimulationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
