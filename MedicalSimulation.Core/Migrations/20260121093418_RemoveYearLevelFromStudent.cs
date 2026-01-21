using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSimulation.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveYearLevelFromStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearLevel",
                table: "Students");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearLevel",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
