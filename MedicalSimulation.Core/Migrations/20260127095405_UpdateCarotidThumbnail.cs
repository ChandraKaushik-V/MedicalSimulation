using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSimulation.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarotidThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(4732));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(7377));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ThumbnailUrl" },
                values: new object[] { new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(7382), "/pictures/cartoid.jpg" });

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(1376));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(2943));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(2949));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 54, 4, 27, DateTimeKind.Utc).AddTicks(2950));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 586, DateTimeKind.Utc).AddTicks(392));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 586, DateTimeKind.Utc).AddTicks(2863));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ThumbnailUrl" },
                values: new object[] { new DateTime(2026, 1, 27, 9, 29, 46, 586, DateTimeKind.Utc).AddTicks(2868), "/pictures/carotid-stenting.png" });

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 585, DateTimeKind.Utc).AddTicks(6668));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 585, DateTimeKind.Utc).AddTicks(8446));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 585, DateTimeKind.Utc).AddTicks(8451));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 585, DateTimeKind.Utc).AddTicks(8452));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 585, DateTimeKind.Utc).AddTicks(8454));
        }
    }
}
