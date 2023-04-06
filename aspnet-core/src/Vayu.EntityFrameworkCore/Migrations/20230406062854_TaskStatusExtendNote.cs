using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class TaskStatusExtendNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "VayuTaskStatuses",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "VayuTaskStatuses");
        }
    }
}
