using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalSimulation.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorSpecializationAndValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Instructors");

            migrationBuilder.AddColumn<int>(
                name: "SpecializationId",
                table: "Instructors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InstructorSpecializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorSpecializations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InstructorSpecializations",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Skin and related conditions", "Dermatology" },
                    { 2, "Brain and nervous system surgery", "Neurosurgery" },
                    { 3, "Heart and cardiovascular system", "Cardiology" }
                });

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 51, 34, 524, DateTimeKind.Utc).AddTicks(2522));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 20, 12, 51, 34, 524, DateTimeKind.Utc).AddTicks(5223));

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_SpecializationId",
                table: "Instructors",
                column: "SpecializationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_InstructorSpecializations_SpecializationId",
                table: "Instructors",
                column: "SpecializationId",
                principalTable: "InstructorSpecializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_InstructorSpecializations_SpecializationId",
                table: "Instructors");

            migrationBuilder.DropTable(
                name: "InstructorSpecializations");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_SpecializationId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
        }
    }
}
