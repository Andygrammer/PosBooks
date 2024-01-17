using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PosBooksConsumer.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    RenterEmail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Clients_RenterEmail",
                        column: x => x.RenterEmail,
                        principalTable: "Clients",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "WaitList",
                columns: table => new
                {
                    RequesterEmail = table.Column<string>(type: "TEXT", nullable: false),
                    BookRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_WaitList_Books_BookRequestId",
                        column: x => x.BookRequestId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaitList_Clients_RequesterEmail",
                        column: x => x.RequesterEmail,
                        principalTable: "Clients",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Publisher", "RenterEmail", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Jorge Amado", "Cia das Letras", null, "Capitães da Areia", 2010 },
                    { 2, "Octavia E. Butler", "Morro Branco", null, "Kindred", 2019 },
                    { 3, "Marília de Camargo César", "Thomas Nelson Brasil", null, "O grito de Eva", 2021 },
                    { 4, "CS Lewis", "Harper Collins", null, "As Crônicas de Nárnia", 2023 },
                    { 5, "Louise Greig", "Harper Collins", null, "O Pequeno Príncipe", 2023 },
                    { 6, "JRR Tokien", "Rocco", null, "O Hobbit", 2019 },
                    { 7, "Margaret Atwood", "Cia das Letras", null, "O conto da aia", 2017 },
                    { 8, "Isaac Asimov", "Aleph", null, "Fundação", 2009 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_RenterEmail",
                table: "Books",
                column: "RenterEmail");

            migrationBuilder.CreateIndex(
                name: "IX_WaitList_BookRequestId",
                table: "WaitList",
                column: "BookRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitList_RequesterEmail",
                table: "WaitList",
                column: "RequesterEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaitList");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
