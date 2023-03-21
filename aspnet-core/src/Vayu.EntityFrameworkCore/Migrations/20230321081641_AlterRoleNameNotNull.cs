using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterRoleNameNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuRoles_Name",
                table: "VayuRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VayuRoles",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VayuRoles_Name",
                table: "VayuRoles",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuRoles_Name",
                table: "VayuRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VayuRoles",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_VayuRoles_Name",
                table: "VayuRoles",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
