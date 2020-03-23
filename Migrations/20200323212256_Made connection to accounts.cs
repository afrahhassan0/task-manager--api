using Microsoft.EntityFrameworkCore.Migrations;

namespace _netCoreBackend.Migrations
{
    public partial class Madeconnectiontoaccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_AdminId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AdminId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AdminId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Groups_AdminId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Groups",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "AdminUsername",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminUsername",
                table: "Groups",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AdminUsername",
                table: "Tasks",
                column: "AdminUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AdminUsername",
                table: "Groups",
                column: "AdminUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Credentials_AdminUsername",
                table: "Groups",
                column: "AdminUsername",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_AdminUsername",
                table: "Tasks",
                column: "AdminUsername",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Credentials_AdminUsername",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Groups_AdminUsername",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "AdminUsername",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AdminUsername",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Groups",
                newName: "description");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AdminId",
                table: "Tasks",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AdminId",
                table: "Groups",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_AdminId",
                table: "Groups",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AdminId",
                table: "Tasks",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
