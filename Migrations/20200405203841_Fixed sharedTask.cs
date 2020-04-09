using Microsoft.EntityFrameworkCore.Migrations;

namespace _netCoreBackend.Migrations
{
    public partial class FixedsharedTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_AssignedBy",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedBy",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "CredentialsUsername",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CredentialsUsername",
                table: "Tasks",
                column: "CredentialsUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_CredentialsUsername",
                table: "Tasks",
                column: "CredentialsUsername",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_CredentialsUsername",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CredentialsUsername",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CredentialsUsername",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "AssignedBy",
                table: "Tasks",
                type: "character varying(15)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedBy",
                table: "Tasks",
                column: "AssignedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_AssignedBy",
                table: "Tasks",
                column: "AssignedBy",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
