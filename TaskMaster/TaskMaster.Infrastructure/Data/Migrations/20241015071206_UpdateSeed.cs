using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMaster.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "59818ae5-37ed-4de2-8d17-61eb431639b7", "98e1c54e-cc2b-4c09-8678-b0fba336e522" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98e1c54e-cc2b-4c09-8678-b0fba336e522");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bd68a836-f988-4dea-af8d-ad33664480af",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "12a94500-f223-4839-9935-2f206247bcc1", "AQAAAAIAAYagAAAAEMACg+9Df5cxAxdAIzxbtsIcbG1xoL2h+cHRhFJjBor1IVrR41uAlOf8UEK//P9IyA==", "86460313-5281-4b81-ae1a-945caed618c4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d9106be3-ab37-4a0b-9fcd-93fa14f6917e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e0ab078-3549-43b2-9657-a0079b760576", "AQAAAAIAAYagAAAAEKbp+6AS8tJj9jidxrtA7oroz6I0KMpmz59u9G7uSWuaDrYskR2nt/478YTWv1Ixbw==", "a5215e58-7018-4848-8fae-bc4d316d8665" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "UserId" },
                values: new object[] { "Add custom font.", "bd68a836-f988-4dea-af8d-ad33664480af" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                column: "Message",
                value: "Added new task!");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bd68a836-f988-4dea-af8d-ad33664480af",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "96346942-b812-4cef-9d68-093072d49dc1", "AQAAAAIAAYagAAAAEIRbS9raCjdi5NPMpbjMpHet1m/zgj8jZoCD70mDJe3lG2luJ1ZOemW/itthg3kiXg==", "8fb4e7cd-3326-4410-99e1-78b202c85781" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d9106be3-ab37-4a0b-9fcd-93fa14f6917e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5005bea9-a157-4a35-974b-1d1f4466e66c", "AQAAAAIAAYagAAAAELQRYFNwuoVaeaasV1o4fq4S8mmZHiH+cEcEKq1kvFu2jkdmANq7Ok2D0nI7J2gtDw==", "667953a0-1aef-4e3d-b8ce-571983ff03d1" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "98e1c54e-cc2b-4c09-8678-b0fba336e522", 0, "a6530765-cca1-4cb9-931e-cfb6ccf39900", "peter@gmail.com", false, false, null, "PETER@GMAIL.COM", "PETER", "AQAAAAIAAYagAAAAEF7y0c3/83j3uYbKor44LO6zmfP62KpSd+bL0cnYJl7MonJLiTNIDs0yOGbU83rZYA==", null, false, "d5420188-d921-48c3-8305-c952389b2d0a", false, "Peter" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "UserId" },
                values: new object[] { "Great idea! Let's discuss this in the next meeting.", "98e1c54e-cc2b-4c09-8678-b0fba336e522" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                column: "Message",
                value: "Peter commented on the task 'Design Landing Page'.");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "59818ae5-37ed-4de2-8d17-61eb431639b7", "98e1c54e-cc2b-4c09-8678-b0fba336e522" });
        }
    }
}
