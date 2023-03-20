using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AddRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "VayuStaffs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "VayuRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VayuRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VayuStaffs_RoleId",
                table: "VayuStaffs",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_VayuRoles_Name",
                table: "VayuRoles",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VayuStaffs_VayuRoles_RoleId",
                table: "VayuStaffs",
                column: "RoleId",
                principalTable: "VayuRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VayuStaffs_VayuRoles_RoleId",
                table: "VayuStaffs");

            migrationBuilder.DropTable(
                name: "VayuRoles");

            migrationBuilder.DropIndex(
                name: "IX_VayuStaffs_RoleId",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "VayuStaffs");
        }
    }
}
