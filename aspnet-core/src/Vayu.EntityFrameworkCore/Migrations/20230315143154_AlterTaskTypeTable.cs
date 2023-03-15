using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterTaskTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "VayuTaskTypes",
                newName: "TaskCategory");

            migrationBuilder.RenameIndex(
                name: "IX_VayuTaskTypes_Name_Type",
                table: "VayuTaskTypes",
                newName: "IX_VayuTaskTypes_Name_TaskCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskCategory",
                table: "VayuTaskTypes",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_VayuTaskTypes_Name_TaskCategory",
                table: "VayuTaskTypes",
                newName: "IX_VayuTaskTypes_Name_Type");
        }
    }
}
