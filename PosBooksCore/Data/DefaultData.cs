using PosBooksCore.Models;

namespace PosBooksCore.Data
{
    public class DefaultData
    {
        public static List<Book> Get() =>
            new List<Book>()
            {
                new Book() { Id= 1, Title = "Capitães da Areia", Author = "Jorge Amado", Publisher = "Cia das Letras", Year = 2010 },
                new Book() { Id= 2, Title = "Kindred", Author = "Octavia E. Butler", Publisher = "Morro Branco", Year = 2019 },
                new Book() { Id= 3, Title = "O grito de Eva", Author = "Marília de Camargo César", Publisher = "Thomas Nelson Brasil", Year = 2021 },
                new Book() { Id= 4, Title = "As Crônicas de Nárnia", Author = "CS Lewis", Publisher = "Harper Collins", Year = 2023 },
                new Book() { Id= 5, Title = "O Pequeno Príncipe", Author = "Louise Greig", Publisher = "Harper Collins", Year = 2023 },
                new Book() { Id= 6, Title = "O Hobbit", Author = "JRR Tokien", Publisher = "Rocco", Year = 2019 },
                new Book() { Id= 7, Title = "O conto da aia", Author = "Margaret Atwood", Publisher = "Cia das Letras", Year = 2017 },
                new Book() { Id= 8, Title = "Fundação", Author = "Isaac Asimov", Publisher = "Aleph", Year = 2009 },
            };
    }
}
