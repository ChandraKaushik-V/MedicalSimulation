using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalSimulation.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Simulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    EstimatedMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalStates = table.Column<int>(type: "int", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Simulations_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurgeryStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimulationId = table.Column<int>(type: "int", nullable: false),
                    StateNumber = table.Column<int>(type: "int", nullable: false),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InteractionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotspotDataJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerOptionsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswerIndex = table.Column<int>(type: "int", nullable: true),
                    LayersJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgeryStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurgeryStates_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    TimeSpent = table.Column<TimeSpan>(type: "time", nullable: false),
                    FeedbackJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VitalSignsHistoryJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailedStepDataJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClinicalErrorsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceMetricsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProgress_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProgress_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Specialties",
                columns: new[] { "Id", "Color", "Description", "DisplayOrder", "IconClass", "IsActive", "Name" },
                values: new object[] { 1, null, "Fundamental techniques for all surgeons", 0, null, true, "Basic Surgical Skills" });

            migrationBuilder.InsertData(
                table: "Simulations",
                columns: new[] { "Id", "CreatedAt", "Description", "Difficulty", "EstimatedMinutes", "IsActive", "SpecialtyId", "ThumbnailUrl", "Title", "TotalStates" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 19, 9, 38, 35, 791, DateTimeKind.Utc).AddTicks(3477), "Master the subcuticular running suture technique with this interactive video simulation.", 2, 15, true, 1, "/pictures/suture.png", "Subcuticular Running Suture", 6 },
                    { 2, new DateTime(2026, 1, 19, 9, 38, 35, 791, DateTimeKind.Utc).AddTicks(5978), "Learn the critical steps and anatomical considerations in performing a posterior neck craniectomy.", 3, 10, true, 1, "/pictures/craniectomy.png", "Posterior Neck Craniectomy", 6 }
                });

            migrationBuilder.InsertData(
                table: "SurgeryStates",
                columns: new[] { "Id", "AnswerOptionsJson", "CorrectAnswerIndex", "Description", "HotspotDataJson", "InteractionType", "LayersJson", "QuestionText", "SimulationId", "StateName", "StateNumber", "VideoUrl" },
                values: new object[,]
                {
                    { 1, null, null, "Click the correct entry point for the first intradermal bite.", "{\"pauseTime\":0.922,\"x\":55.21,\"y\":52.76,\"radius\":10}", "click-hotspot", "[]", "Click the correct entry point for the first intradermal bite.", 1, "First Intradermal Bite Entry", 1, "/videos/simulations/subcuticular-running-suture.mp4" },
                    { 2, null, null, "Click where the needle should exit on the opposite side.", "{\"pauseTime\":6.423,\"x\":50,\"y\":72.76,\"radius\":10}", "click-hotspot", "[]", "Click where the needle should exit on the opposite side.", 1, "Needle Exit Point", 2, "/videos/simulations/subcuticular-running-suture.mp4" },
                    { 3, null, null, "Click where the suture loop should rest before tightening.", "{\"pauseTime\":12.13,\"x\":48.81,\"y\":63.01,\"radius\":10}", "click-hotspot", "[]", "Click where the suture loop should rest before tightening.", 1, "Suture Loop Position", 3, "/videos/simulations/subcuticular-running-suture.mp4" },
                    { 4, null, null, "Where should the cut be made?", "{\"pauseTime\":51.449,\"x\":57.68,\"y\":27.57,\"radius\":10}", "click-hotspot", "[]", "Where should the cut be made?", 1, "Cut Placement", 4, "/videos/simulations/subcuticular-running-suture.mp4" },
                    { 5, "[\"Begin the next intradermal bite at equal spacing\",\"Cut the suture ends\",\"Tighten further to blanch the skin\",\"Place a superficial interrupted stitch\"]", 0, "What is the next correct step after securing this knot in a running subcuticular suture?", "{\"pauseTime\":57.000}", "mcq", "[]", "What is the next correct step after securing this knot in a running subcuticular suture?", 1, "Next Step After Knot", 5, "/videos/simulations/subcuticular-running-suture.mp4" },
                    { 6, null, null, "Click the correct point to place the next intradermal bite.", "{\"pauseTime\":60,\"x\":42.32,\"y\":58.94,\"radius\":10}", "click-hotspot", "[]", "Click the correct point to place the next intradermal bite.", 1, "Next Intradermal Bite", 6, "/videos/simulations/subcuticular-running-suture.mp4" },
                    { 7, null, null, "Click on the correct incision line for a posterior neck craniectomy.", "{\"pauseTime\":3.765,\"x\":50.46,\"y\":38.95,\"radius\":5}", "click-hotspot", "[]", "Click on the correct incision line for a posterior neck craniectomy.", 2, "Incision Line Identification", 1, "/videos/simulations/craniectomy.mp4" },
                    { 8, "[\"Skin and subcutaneous tissue\",\"Trapezius muscle\",\"Deep cervical fascia and underlying vital structures\",\"Vertebral spinous processes\"]", 2, "Identify the critical structure before proceeding.", "{\"pauseTime\":16.000}", "mcq", "[]", "Which structure must be clearly identified before proceeding further in a posterior neck carcinectomy?", 2, "Structure Identification", 2, "/videos/simulations/craniectomy.mp4" },
                    { 9, "[\"Excise only the visibly abnormal tissue\",\"Cut as close as possible to minimize tissue loss\",\"Include an adequate margin of healthy tissue around the lesion\",\"Delay margin planning until closure\"]", 2, "Understanding the correct principle for defining resection margins.", "{\"pauseTime\":28.000}", "mcq", "[]", "What is the correct principle for defining the resection margin during a carcinectomy?", 2, "Resection Margin Principles", 3, "/videos/simulations/craniectomy.mp4" },
                    { 10, null, null, "Point where the next incision is to be done.", "{\"pauseTime\":56.669,\"x\":49.91,\"y\":40.25,\"radius\":5}", "click-hotspot", "[]", "Point where the next incision is to be done.", 2, "Next Incision Point", 4, "/videos/simulations/craniectomy.mp4" },
                    { 11, "[\"Skin flap\",\"Paraspinal muscles\",\"Spinal cord and dura mater\",\"Trachea\"]", 2, "Identify the structure that must be actively protected during dissection.", "{\"pauseTime\":81.431}", "mcq", "[]", "Which structure must be actively protected while performing this dissection?", 2, "Structure Protection", 5, "/videos/simulations/craniectomy.mp4" },
                    { 12, "[\"Immediate skin closure with sutures\",\"Irrigation and leave the wound open\",\"Apply a sterile pressure dressing / protective dressing\",\"Begin deep muscle reconstruction\"]", 2, "Determine the appropriate next step after excising the lesion.", "{\"pauseTime\":93.431}", "mcq", "[]", "What is the next appropriate step after excising the lesion?", 2, "Post-Excision Management", 6, "/videos/simulations/craniectomy.mp4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Simulations_SpecialtyId",
                table: "Simulations",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryStates_SimulationId",
                table: "SurgeryStates",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgress_SimulationId",
                table: "UserProgress",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgress_UserId",
                table: "UserProgress",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "SurgeryStates");

            migrationBuilder.DropTable(
                name: "UserProgress");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Simulations");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}
