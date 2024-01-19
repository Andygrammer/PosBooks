using Microsoft.EntityFrameworkCore;
using PosBooksCore.Data;
using PosBooksCore.Models;

namespace PosBooks.Model;

public class PosBookContext : DbContext
{
    public PosBookContext(DbContextOptions<PosBookContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>().HasData(DefaultData.Get());
        modelBuilder.Entity<Book>().Ignore(b => b.Renter);
        modelBuilder.Entity<Book>().HasKey(b => b.Id);
    }

    public DbSet<Book> Books { get; set; }
}
