using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterTaskRemoveName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuTasks_Name_TaskTypeId",
                table: "VayuTasks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VayuTasks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompleteDateTime",
                table: "VayuTasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_CompleteDateTime",
                table: "VayuTasks",
                column: "CompleteDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_CreateDateTime",
                table: "VayuTasks",
                column: "CreateDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_CreateDateTime_CompleteDateTime",
                table: "VayuTasks",
                columns: new[] { "CreateDateTime", "CompleteDateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuTasks_CompleteDateTime",
                table: "VayuTasks");

            migrationBuilder.DropIndex(
                name: "IX_VayuTasks_CreateDateTime",
                table: "VayuTasks");

            migrationBuilder.DropIndex(
                name: "IX_VayuTasks_CreateDateTime_CompleteDateTime",
                table: "VayuTasks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompleteDateTime",
                table: "VayuTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VayuTasks",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_Name_TaskTypeId",
                table: "VayuTasks",
                columns: new[] { "Name", "TaskTypeId" });
        }
    }
}
