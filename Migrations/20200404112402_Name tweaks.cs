using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using _netCoreBackend.Models.Enums;

namespace _netCoreBackend.Migrations
{
    public partial class Nametweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedTaskAssignments_Tasks_SharedTaskId",
                table: "SharedTaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_SharedTasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SharedTasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AdminUsername",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SharedTasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SharedTaskId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tasks");

            migrationBuilder.AlterColumn<Status>(
                name: "Status",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(Status),
                oldType: "status",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedBy",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Tasks",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedBy",
                table: "Tasks",
                column: "AssignedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedTaskAssignments_Tasks_SharedTaskId",
                table: "SharedTaskAssignments",
                column: "SharedTaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_OwnerId",
                table: "Tasks",
                column: "OwnerId",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_AssignedBy",
                table: "Tasks",
                column: "AssignedBy",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedTaskAssignments_Tasks_SharedTaskId",
                table: "SharedTaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_OwnerId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_AssignedBy",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedBy",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Tasks");

            migrationBuilder.AlterColumn<Status>(
                name: "Status",
                table: "Tasks",
                type: "status",
                nullable: true,
                oldClrType: typeof(Status));

            migrationBuilder.AddColumn<string>(
                name: "AdminUsername",
                table: "Tasks",
                type: "character varying(15)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SharedTasks_AdminUsername",
                table: "Tasks",
                type: "character varying(15)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SharedTaskId",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "CreatedDate",
                table: "Tasks",
                type: "varchar(15)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "SharedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AdminUsername",
                table: "Tasks",
                column: "AdminUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SharedTasks_AdminUsername",
                table: "Tasks",
                column: "SharedTasks_AdminUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedTaskAssignments_Tasks_SharedTaskId",
                table: "SharedTaskAssignments",
                column: "SharedTaskId",
                principalTable: "Tasks",
                principalColumn: "SharedTaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_AdminUsername",
                table: "Tasks",
                column: "AdminUsername",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_SharedTasks_AdminUsername",
                table: "Tasks",
                column: "SharedTasks_AdminUsername",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
