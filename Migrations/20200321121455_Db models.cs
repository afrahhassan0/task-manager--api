using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using _netCoreBackend.Models.Enums;
using _netCoreBackend.Models.Objects;

namespace _netCoreBackend.Migrations
{
    public partial class Dbmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role", "admin,member")
                .Annotation("Npgsql:Enum:status", "open,doing,reviewing,done");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(nullable: false),
                    Role = table.Column<Role>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    About = table.Column<string>(nullable: true),
                    CustomBackgroundColor = table.Column<string>(nullable: true, defaultValue: "#e1e1e1"),
                    Discriminator = table.Column<string>(nullable: false),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Username = table.Column<string>(maxLength: 15, nullable: false),
                    Password = table.Column<string>(maxLength: 200, nullable: false),
                    OrganizationName = table.Column<string>(maxLength: 50, nullable: false),
                    User_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Credentials_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(maxLength: 25, nullable: false),
                    CreatedDate = table.Column<string>(type: "varchar(10)", nullable: false),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    AdminId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    SharedTaskId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<string>(type: "varchar(15)", nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Status = table.Column<Status>(nullable: true),
                    Checklists = table.Column<List<Checklist>>(type: "jsonb", nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    OwnerId = table.Column<int>(nullable: true),
                    Deadline = table.Column<string>(type: "varchar(15)", nullable: true),
                    AdminComments = table.Column<string[]>(nullable: true),
                    AdminId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.SharedTaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Member_ID = table.Column<int>(nullable: false),
                    Group_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => new { x.Group_ID, x.Member_ID });
                    table.ForeignKey(
                        name: "FK_Memberships_Groups_Group_ID",
                        column: x => x.Group_ID,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Users_Member_ID",
                        column: x => x.Member_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedTaskAssignments",
                columns: table => new
                {
                    SharedTaskId = table.Column<int>(nullable: false),
                    MemberShipUserId = table.Column<int>(nullable: false),
                    MembershipGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedTaskAssignments", x => new { x.SharedTaskId, x.MembershipGroupId, x.MemberShipUserId });
                    table.ForeignKey(
                        name: "FK_SharedTaskAssignments_Tasks_SharedTaskId",
                        column: x => x.SharedTaskId,
                        principalTable: "Tasks",
                        principalColumn: "SharedTaskId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedTaskAssignments_Memberships_MembershipGroupId_MemberS~",
                        columns: x => new { x.MembershipGroupId, x.MemberShipUserId },
                        principalTable: "Memberships",
                        principalColumns: new[] { "Group_ID", "Member_ID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_User_Id",
                table: "Credentials",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AdminId",
                table: "Groups",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_Member_ID",
                table: "Memberships",
                column: "Member_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SharedTaskAssignments_MembershipGroupId_MemberShipUserId",
                table: "SharedTaskAssignments",
                columns: new[] { "MembershipGroupId", "MemberShipUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AdminId",
                table: "Tasks",
                column: "AdminId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "SharedTaskAssignments");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
