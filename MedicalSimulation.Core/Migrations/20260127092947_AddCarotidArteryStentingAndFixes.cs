using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSimulation.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCarotidArteryStentingAndFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 29, 46, 586, DateTimeKind.Utc).AddTicks(2868));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(7162));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "Simulations",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(3788));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(5352));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(5356));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(5358));

            migrationBuilder.UpdateData(
                table: "ValidInstructorEmployeeIds",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 27, 9, 26, 9, 216, DateTimeKind.Utc).AddTicks(5360));
        }
    }
}
