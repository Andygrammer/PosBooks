using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosBooksConsumer.Models
{
    public class PBCContext : DbContext
    {
        public PBCContext(DbContextOptions<PBCContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasData(new List<Book>()
            {
                new Book() { Id= 1, Title = "Capitães da Areia", Author = "Jorge Amado", Publisher = "Cia das Letras", Year = 2010 },
                new Book() { Id= 2, Title = "Kindred", Author = "Octavia E. Butler", Publisher = "Morro Branco", Year = 2019 },
                new Book() { Id= 3, Title = "O grito de Eva", Author = "Marília de Camargo César", Publisher = "Thomas Nelson Brasil", Year = 2021 },
                new Book() { Id= 4, Title = "As Crônicas de Nárnia", Author = "CS Lewis", Publisher = "Harper Collins", Year = 2023 },
                new Book() { Id= 5, Title = "O Pequeno Príncipe", Author = "Louise Greig", Publisher = "Harper Collins", Year = 2023 },
                new Book() { Id= 6, Title = "O Hobbit", Author = "JRR Tokien", Publisher = "Rocco", Year = 2019 },
                new Book() { Id= 7, Title = "O conto da aia", Author = "Margaret Atwood", Publisher = "Cia das Letras", Year = 2017 },
                new Book() { Id= 8, Title = "Fundação", Author = "Isaac Asimov", Publisher = "Aleph", Year = 2009 },
            });

            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<Client>().HasKey(b => b.Email);
            modelBuilder.Entity<WaitList>().HasNoKey();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("DataSource=memory.db;Cache=Shared");

        public DbSet<Book> Books { get; set; }

        public DbSet<WaitList> WaitList { get; set; }
        
        public DbSet<Client> Clients { get; set; }

    }
}
