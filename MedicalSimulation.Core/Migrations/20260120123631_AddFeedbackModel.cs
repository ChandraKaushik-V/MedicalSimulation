using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSimulation.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProgressId = table.Column<int>(type: "int", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeedbackText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_AspNetUsers_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Feedbacks_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Simulations_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Feedbacks_UserProgress_UserProgressId",
                        column: x => x.UserProgressId,
                        principalTable: "UserProgress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 36, 30, 12, DateTimeKind.Utc).AddTicks(4774));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 36, 30, 12, DateTimeKind.Utc).AddTicks(7729));

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_InstructorId",
                table: "Feedbacks",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SimulationId",
                table: "Feedbacks",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_StudentId",
                table: "Feedbacks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserProgressId",
                table: "Feedbacks",
                column: "UserProgressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 10, 8, 9, 402, DateTimeKind.Utc).AddTicks(5480));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 10, 8, 9, 402, DateTimeKind.Utc).AddTicks(8053));
        }
    }
}
