using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PosBooks.Migrations
{
    /// <inheritdoc />
    public partial class migrationinicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Publisher", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Jorge Amado", "Cia das Letras", "Capitães da Areia", 2010 },
                    { 2, "Octavia E. Butler", "Morro Branco", "Kindred", 2019 },
                    { 3, "Marília de Camargo César", "Thomas Nelson Brasil", "O grito de Eva", 2021 },
                    { 4, "CS Lewis", "Harper Collins", "As Crônicas de Nárnia", 2023 },
                    { 5, "Louise Greig", "Harper Collins", "O Pequeno Príncipe", 2023 },
                    { 6, "JRR Tokien", "Rocco", "O Hobbit", 2019 },
                    { 7, "Margaret Atwood", "Cia das Letras", "O conto da aia", 2017 },
                    { 8, "Isaac Asimov", "Aleph", "Fundação", 2009 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
