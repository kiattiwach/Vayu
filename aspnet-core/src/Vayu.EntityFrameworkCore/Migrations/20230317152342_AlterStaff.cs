using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "VayuStaffs",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "VayuStaffs",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "VayuStaffs",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonNumber",
                table: "VayuStaffs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "VayuStaffs",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadioCode",
                table: "VayuStaffs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs",
                columns: new[] { "Code", "RadioCode" },
                unique: true,
                filter: "[Code] IS NOT NULL AND [RadioCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "ContactPersonNumber",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "RadioCode",
                table: "VayuStaffs");
        }
    }
}
