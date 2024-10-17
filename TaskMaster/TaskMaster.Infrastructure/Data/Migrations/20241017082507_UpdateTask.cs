using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMaster.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedTime",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "The time when the task was completed");

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

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CompletedTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedTime",
                table: "Tasks");

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
        }
    }
}
