using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterStaffIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs");

            migrationBuilder.CreateIndex(
                name: "IX_VayuStaffs_Code",
                table: "VayuStaffs",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VayuStaffs_Code",
                table: "VayuStaffs");

            migrationBuilder.CreateIndex(
                name: "IX_VayuStaffs_Code_RadioCode",
                table: "VayuStaffs",
                columns: new[] { "Code", "RadioCode" },
                unique: true,
                filter: "[RadioCode] IS NOT NULL");
        }
    }
}
