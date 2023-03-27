using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class TypeSurname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "VayuStaffs",
                newName: "Surname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "VayuStaffs",
                newName: "SurName");
        }
    }
}
