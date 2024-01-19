using Microsoft.EntityFrameworkCore;
using PosBooksCore.Data;
using PosBooksCore.Models;

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

            modelBuilder.Entity<Book>().HasData(DefaultData.Get());
            modelBuilder.Entity<Book>().HasKey(b => b.Id);

            modelBuilder.Entity<Client>().HasKey(b => b.Email);
            modelBuilder.Entity<Client>().Property(b => b.Email).HasMaxLength(150).ValueGeneratedNever();

            modelBuilder.Entity<WaitList>().HasKey( new string[] { "BookRequestId", "RequesterEmail"});
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<WaitList> WaitList { get; set; }
        
        public DbSet<Client> Clients { get; set; }

    }
}
