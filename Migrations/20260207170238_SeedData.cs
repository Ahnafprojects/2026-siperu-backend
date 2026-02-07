using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SiperuBackend.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "CreatedAt", "Description", "IsAvailable", "Name" },
                values: new object[,]
                {
                    { 1, 30, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang Kelas Lantai 1", true, "C-101" },
                    { 2, 40, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang Kelas Lantai 1", true, "C-102" },
                    { 3, 25, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Lab dengan 25 PC", true, "Lab Komputer 1" },
                    { 4, 200, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Aula untuk acara besar", true, "Aula" },
                    { 5, 15, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang untuk rapat kecil", true, "Ruang Rapat" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "EndTime", "Purpose", "RoomId", "StartTime", "Status", "StudentName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 9, 12, 0, 0, 0, DateTimeKind.Unspecified), "Rapat Himpunan", 1, new DateTime(2026, 2, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), "Approved", "Ahmad Fauzi" },
                    { 2, new DateTime(2026, 2, 10, 16, 0, 0, 0, DateTimeKind.Unspecified), "Workshop Programming", 3, new DateTime(2026, 2, 10, 13, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "Siti Nurhaliza" },
                    { 3, new DateTime(2026, 2, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), "Seminar Mahasiswa", 4, new DateTime(2026, 2, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), "Approved", "Budi Santoso" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
