using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskMaster.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "37e2ae4c-9365-41f3-8c3e-ad2894cada60", null, "Admin", "ADMIN" },
                    { "59818ae5-37ed-4de2-8d17-61eb431639b7", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "98e1c54e-cc2b-4c09-8678-b0fba336e522", 0, "a6530765-cca1-4cb9-931e-cfb6ccf39900", "peter@gmail.com", false, false, null, "PETER@GMAIL.COM", "PETER", "AQAAAAIAAYagAAAAEF7y0c3/83j3uYbKor44LO6zmfP62KpSd+bL0cnYJl7MonJLiTNIDs0yOGbU83rZYA==", null, false, "d5420188-d921-48c3-8305-c952389b2d0a", false, "Peter" },
                    { "bd68a836-f988-4dea-af8d-ad33664480af", 0, "96346942-b812-4cef-9d68-093072d49dc1", "john@gmail.com", false, false, null, "JOHN@GMAIL.COM", "JOHN", "AQAAAAIAAYagAAAAEIRbS9raCjdi5NPMpbjMpHet1m/zgj8jZoCD70mDJe3lG2luJ1ZOemW/itthg3kiXg==", null, false, "8fb4e7cd-3326-4410-99e1-78b202c85781", false, "John" },
                    { "d9106be3-ab37-4a0b-9fcd-93fa14f6917e", 0, "5005bea9-a157-4a35-974b-1d1f4466e66c", "admin@gmail.com", false, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAELQRYFNwuoVaeaasV1o4fq4S8mmZHiH+cEcEKq1kvFu2jkdmANq7Ok2D0nI7J2gtDw==", null, false, "667953a0-1aef-4e3d-b8ce-571983ff03d1", false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "59818ae5-37ed-4de2-8d17-61eb431639b7", "98e1c54e-cc2b-4c09-8678-b0fba336e522" },
                    { "59818ae5-37ed-4de2-8d17-61eb431639b7", "bd68a836-f988-4dea-af8d-ad33664480af" },
                    { "37e2ae4c-9365-41f3-8c3e-ad2894cada60", "d9106be3-ab37-4a0b-9fcd-93fa14f6917e" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "DateSent", "Message", "UserId" },
                values: new object[] { 1, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Peter commented on the task 'Design Landing Page'.", "bd68a836-f988-4dea-af8d-ad33664480af" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Description", "DueTime", "Priority", "Status", "Title", "UserId" },
                values: new object[] { 1, "Create a modern landing page for the website.", new DateTime(2025, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medium", "ToDo", "Design Landing Page", "bd68a836-f988-4dea-af8d-ad33664480af" });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "DateSent", "TaskId", "UserId" },
                values: new object[] { 1, "Great idea! Let's discuss this in the next meeting.", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "98e1c54e-cc2b-4c09-8678-b0fba336e522" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "59818ae5-37ed-4de2-8d17-61eb431639b7", "98e1c54e-cc2b-4c09-8678-b0fba336e522" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "59818ae5-37ed-4de2-8d17-61eb431639b7", "bd68a836-f988-4dea-af8d-ad33664480af" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "37e2ae4c-9365-41f3-8c3e-ad2894cada60", "d9106be3-ab37-4a0b-9fcd-93fa14f6917e" });

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37e2ae4c-9365-41f3-8c3e-ad2894cada60");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59818ae5-37ed-4de2-8d17-61eb431639b7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98e1c54e-cc2b-4c09-8678-b0fba336e522");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d9106be3-ab37-4a0b-9fcd-93fa14f6917e");

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bd68a836-f988-4dea-af8d-ad33664480af");
        }
    }
}
