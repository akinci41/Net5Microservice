using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Directory.Migrations
{
    public partial class DirectoryDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    Surname = table.Column<string>(type: "varchar(100)", nullable: true),
                    FirmName = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Communications",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactID = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "char(1)", nullable: true),
                    Content = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Communications_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contacts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communications_ContactID",
                table: "Communications",
                column: "ContactID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Communications");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
