using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vayu.Migrations
{
    /// <inheritdoc />
    public partial class InitialVayuEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VayuStaffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CodeName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VayuStaffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VayuTaskTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VayuTaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VayuTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    TaskTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VayuTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VayuTasks_VayuStaffs_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "VayuStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VayuTasks_VayuTaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "VayuTaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VayuTaskStaffs",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VayuTaskStaffs", x => new { x.TaskId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_VayuTaskStaffs_VayuStaffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "VayuStaffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VayuTaskStaffs_VayuTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "VayuTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_LeaderId",
                table: "VayuTasks",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_Name_TaskTypeId",
                table: "VayuTasks",
                columns: new[] { "Name", "TaskTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VayuTasks_TaskTypeId",
                table: "VayuTasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTaskStaffs_StaffId",
                table: "VayuTaskStaffs",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_VayuTaskStaffs_TaskId_StaffId",
                table: "VayuTaskStaffs",
                columns: new[] { "TaskId", "StaffId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VayuTaskTypes_Name_Type",
                table: "VayuTaskTypes",
                columns: new[] { "Name", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VayuTaskStaffs");

            migrationBuilder.DropTable(
                name: "VayuTasks");

            migrationBuilder.DropTable(
                name: "VayuStaffs");

            migrationBuilder.DropTable(
                name: "VayuTaskTypes");
        }
    }
}
