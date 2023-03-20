using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterIndexStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "VayuStaffs",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs",
                columns: new[] { "Code", "RadioCode" },
                unique: true,
                filter: "[RadioCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "VayuStaffs",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.CreateIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs",
                columns: new[] { "Code", "RadioCode" },
                unique: true,
                filter: "[Code] IS NOT NULL AND [RadioCode] IS NOT NULL");
        }
    }
}
