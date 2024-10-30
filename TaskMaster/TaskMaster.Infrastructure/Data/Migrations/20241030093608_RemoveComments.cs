using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMaster.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bd68a836-f988-4dea-af8d-ad33664480af",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67b4a36c-e7e3-49d1-911e-b125519ad4a5", "AQAAAAIAAYagAAAAEGa1Pu6OoIcJp04cH/o796TBx/wWb7COUKDlS2HaFQckp0KaGPofypX9Fb+K1emwJA==", "ef80c867-7d83-4355-b8b2-9677cb41bfd2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d9106be3-ab37-4a0b-9fcd-93fa14f6917e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ae3fa10c-b123-454f-b63c-27199ac58809", "AQAAAAIAAYagAAAAECPVHsY+uvcLjYrHjLnCy1kslrzUlaPPdjLW+Mw5MBZxzkuNTuZUNGiUJQEkydEAeQ==", "e6cc6073-6dbd-4ae2-83c0-48f63a098585" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the comment")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false, comment: "Task identifier"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User identifier"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The content of the comment"),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date when the comment was posted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents a comment added to a task");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bd68a836-f988-4dea-af8d-ad33664480af",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c871c7da-8700-44e8-bf4f-65b1c342236b", "AQAAAAIAAYagAAAAEMDKig7bYrOEMf5K02WDxT7gK7sc30kdfkJ8qBJDzGDWqm/n59fqodMQTGEDH8z1mA==", "273c2c0d-d31f-4125-872e-2e37e86b9e6b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d9106be3-ab37-4a0b-9fcd-93fa14f6917e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e001f49c-dd69-4466-9fe4-c98a4d6508ae", "AQAAAAIAAYagAAAAECVmaI78W9RNOG0Xlio7En84y1lGI1oB7KzPknZYEhlGcqAVrMLKz/cXN/Aux5Ovaw==", "52782f16-6715-4862-ac1c-0c12428118c8" });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "DateSent", "TaskId", "UserId" },
                values: new object[] { 1, "Add custom font.", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "bd68a836-f988-4dea-af8d-ad33664480af" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TaskId",
                table: "Comments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }
    }
}
