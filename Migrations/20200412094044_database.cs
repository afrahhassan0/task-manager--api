using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using _netCoreBackend.Models.Enums;
using _netCoreBackend.Models.Objects;

namespace _netCoreBackend.Migrations
{
    public partial class database : Migration
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
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
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
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedDate = table.Column<string>(type: "varchar(10)", nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    AdminUsername = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Credentials_AdminUsername",
                        column: x => x.AdminUsername,
                        principalTable: "Credentials",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MemberUsername = table.Column<string>(nullable: false),
                    GroupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => new { x.GroupID, x.MemberUsername });
                    table.ForeignKey(
                        name: "FK_Memberships_Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Credentials_MemberUsername",
                        column: x => x.MemberUsername,
                        principalTable: "Credentials",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    Deadline = table.Column<string>(type: "varchar(15)", nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Status = table.Column<Status>(nullable: false),
                    Checklists = table.Column<List<Checklist>>(type: "jsonb", nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    CredentialsUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Credentials_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Credentials",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Credentials_CredentialsUsername",
                        column: x => x.CredentialsUsername,
                        principalTable: "Credentials",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_User_Id",
                table: "Credentials",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AdminUsername",
                table: "Groups",
                column: "AdminUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberUsername",
                table: "Memberships",
                column: "MemberUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CredentialsUsername",
                table: "Tasks",
                column: "CredentialsUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_GroupId",
                table: "Tasks",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
