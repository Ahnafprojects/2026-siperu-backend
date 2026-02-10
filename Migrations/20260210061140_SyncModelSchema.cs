using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SiperuBackend.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "CreatedAt", "Description", "IsAvailable", "Name" },
                values: new object[,]
                {
                    { 6, 30, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Lab Multimedia", true, "Lab Komputer 2" },
                    { 7, 100, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Untuk seminar dan presentasi", true, "Ruang Seminar" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
