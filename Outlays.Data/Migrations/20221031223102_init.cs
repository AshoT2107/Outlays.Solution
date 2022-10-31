using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Outlays.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    Step = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Outlays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outlays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outlays_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Outlays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outlays_RoomId",
                table: "Outlays",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Outlays_UserId",
                table: "Outlays",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoomId",
                table: "Users",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outlays");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
