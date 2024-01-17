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

            modelBuilder.Entity<ConsumerBook>().HasData(DefaultData.Get());
            modelBuilder.Entity<ConsumerBook>().HasKey(b => b.Id);
            modelBuilder.Entity<Client>().HasKey(b => b.Email);
            modelBuilder.Entity<WaitList>().HasNoKey();
        }

        public DbSet<ConsumerBook> Books { get; set; }

        public DbSet<WaitList> WaitList { get; set; }
        
        public DbSet<Client> Clients { get; set; }

    }
}
