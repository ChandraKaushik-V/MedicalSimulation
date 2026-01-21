using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalSimulation.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddValidInstructorEmployeeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Instructors");

            migrationBuilder.AddColumn<int>(
                name: "ValidEmployeeIdId",
                table: "Instructors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ValidInstructorEmployeeIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidInstructorEmployeeIds", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 17, 13, 48, 148, DateTimeKind.Utc).AddTicks(4004));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 17, 13, 48, 148, DateTimeKind.Utc).AddTicks(6484));

            migrationBuilder.InsertData(
                table: "ValidInstructorEmployeeIds",
                columns: new[] { "Id", "CreatedAt", "EmployeeId", "FirstName", "IsActive", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 21, 17, 13, 48, 148, DateTimeKind.Utc).AddTicks(625), "EMP001", "John", true, "Doe" },
                    { 2, new DateTime(2026, 1, 21, 17, 13, 48, 148, DateTimeKind.Utc).AddTicks(2126), "EMP002", "Jane", true, "Smith" },
                    { 3, new DateTime(2026, 1, 21, 17, 13, 48, 148, DateTimeKind.Utc).AddTicks(2130), "EMP003", "Michael", true, "Johnson" },
                    { 4, new DateTime(2026, 1, 21, 17, 13, 48, 148, DateTimeKind.Utc).AddTicks(2132), "EMP004", "Sarah", true, "Williams" },
                    { 5, new DateTime(2026, 1, 21, 17, 13, 48, 148, DateTimeKind.Utc).AddTicks(2180), "EMP005", "David", true, "Brown" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_ValidEmployeeIdId",
                table: "Instructors",
                column: "ValidEmployeeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_ValidInstructorEmployeeIds_ValidEmployeeIdId",
                table: "Instructors",
                column: "ValidEmployeeIdId",
                principalTable: "ValidInstructorEmployeeIds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_ValidInstructorEmployeeIds_ValidEmployeeIdId",
                table: "Instructors");

            migrationBuilder.DropTable(
                name: "ValidInstructorEmployeeIds");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_ValidEmployeeIdId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "ValidEmployeeIdId",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 9, 34, 17, 43, DateTimeKind.Utc).AddTicks(4777));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 21, 9, 34, 17, 43, DateTimeKind.Utc).AddTicks(7598));
        }
    }
}
