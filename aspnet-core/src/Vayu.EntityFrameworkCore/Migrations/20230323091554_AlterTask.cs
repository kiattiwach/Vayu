using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuTasks_Name_TaskTypeId",
                table: "VayuTasks");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_Name_TaskTypeId",
                table: "VayuTasks",
                columns: new[] { "Name", "TaskTypeId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuTasks_Name_TaskTypeId",
                table: "VayuTasks");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_Name_TaskTypeId",
                table: "VayuTasks",
                columns: new[] { "Name", "TaskTypeId" },
                unique: true);
        }
    }
}
