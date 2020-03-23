using Microsoft.EntityFrameworkCore.Migrations;

namespace _netCoreBackend.Migrations
{
    public partial class Oksotheusercanhavemanyaccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Groups_Group_ID",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Users_Member_ID",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedTaskAssignments_Memberships_MembershipGroupId_MemberS~",
                table: "SharedTaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_OwnerId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SharedTaskAssignments",
                table: "SharedTaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SharedTaskAssignments_MembershipGroupId_MemberShipUserId",
                table: "SharedTaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_Member_ID",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "MemberShipUserId",
                table: "SharedTaskAssignments");

            migrationBuilder.DropColumn(
                name: "Group_ID",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "Member_ID",
                table: "Memberships");

            migrationBuilder.AddColumn<string>(
                name: "SharedTasks_AdminUsername",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberShipUserUsername",
                table: "SharedTaskAssignments",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "Memberships",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MemberUsername",
                table: "Memberships",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SharedTaskAssignments",
                table: "SharedTaskAssignments",
                columns: new[] { "SharedTaskId", "MembershipGroupId", "MemberShipUserUsername" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships",
                columns: new[] { "GroupID", "MemberUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SharedTasks_AdminUsername",
                table: "Tasks",
                column: "SharedTasks_AdminUsername");

            migrationBuilder.CreateIndex(
                name: "IX_SharedTaskAssignments_MembershipGroupId_MemberShipUserUsern~",
                table: "SharedTaskAssignments",
                columns: new[] { "MembershipGroupId", "MemberShipUserUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberUsername",
                table: "Memberships",
                column: "MemberUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Groups_GroupID",
                table: "Memberships",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Credentials_MemberUsername",
                table: "Memberships",
                column: "MemberUsername",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedTaskAssignments_Memberships_MembershipGroupId_MemberS~",
                table: "SharedTaskAssignments",
                columns: new[] { "MembershipGroupId", "MemberShipUserUsername" },
                principalTable: "Memberships",
                principalColumns: new[] { "GroupID", "MemberUsername" },
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Groups_GroupID",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Credentials_MemberUsername",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedTaskAssignments_Memberships_MembershipGroupId_MemberS~",
                table: "SharedTaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Credentials_SharedTasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SharedTasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SharedTaskAssignments",
                table: "SharedTaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SharedTaskAssignments_MembershipGroupId_MemberShipUserUsern~",
                table: "SharedTaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberUsername",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "SharedTasks_AdminUsername",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "MemberShipUserUsername",
                table: "SharedTaskAssignments");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MemberUsername",
                table: "Memberships");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberShipUserId",
                table: "SharedTaskAssignments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Group_ID",
                table: "Memberships",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Member_ID",
                table: "Memberships",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SharedTaskAssignments",
                table: "SharedTaskAssignments",
                columns: new[] { "SharedTaskId", "MembershipGroupId", "MemberShipUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships",
                columns: new[] { "Group_ID", "Member_ID" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedTaskAssignments_MembershipGroupId_MemberShipUserId",
                table: "SharedTaskAssignments",
                columns: new[] { "MembershipGroupId", "MemberShipUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_Member_ID",
                table: "Memberships",
                column: "Member_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Groups_Group_ID",
                table: "Memberships",
                column: "Group_ID",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Users_Member_ID",
                table: "Memberships",
                column: "Member_ID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedTaskAssignments_Memberships_MembershipGroupId_MemberS~",
                table: "SharedTaskAssignments",
                columns: new[] { "MembershipGroupId", "MemberShipUserId" },
                principalTable: "Memberships",
                principalColumns: new[] { "Group_ID", "Member_ID" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_OwnerId",
                table: "Tasks",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Credentials_AdminUsername",
                table: "Tasks",
                column: "AdminUsername",
                principalTable: "Credentials",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
