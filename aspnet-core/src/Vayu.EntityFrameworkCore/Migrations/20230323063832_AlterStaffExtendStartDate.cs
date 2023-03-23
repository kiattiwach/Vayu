using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    public partial class AlterStaffExtendStartDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PassAssessmentDate",
                table: "VayuStaffs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "VayuStaffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1973, 1, 1, 7, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassAssessmentDate",
                table: "VayuStaffs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "VayuStaffs");
        }
    }
}
