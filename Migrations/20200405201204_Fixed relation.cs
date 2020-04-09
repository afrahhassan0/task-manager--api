using Microsoft.EntityFrameworkCore.Migrations;

namespace _netCoreBackend.Migrations
{
    public partial class Fixedrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedTaskAssignments");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Groups",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_GroupId",
                table: "Tasks",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Groups_GroupId",
                table: "Tasks",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Groups_GroupId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_GroupId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Groups",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "SharedTaskAssignments",
                columns: table => new
                {
                    SharedTaskId = table.Column<int>(type: "integer", nullable: false),
                    MembershipGroupId = table.Column<int>(type: "integer", nullable: false),
                    MemberShipUserUsername = table.Column<string>(type: "character varying(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedTaskAssignments", x => new { x.SharedTaskId, x.MembershipGroupId, x.MemberShipUserUsername });
                    table.ForeignKey(
                        name: "FK_SharedTaskAssignments_Tasks_SharedTaskId",
                        column: x => x.SharedTaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedTaskAssignments_Memberships_MembershipGroupId_MemberS~",
                        columns: x => new { x.MembershipGroupId, x.MemberShipUserUsername },
                        principalTable: "Memberships",
                        principalColumns: new[] { "GroupID", "MemberUsername" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedTaskAssignments_MembershipGroupId_MemberShipUserUsern~",
                table: "SharedTaskAssignments",
                columns: new[] { "MembershipGroupId", "MemberShipUserUsername" });
        }
    }
}
