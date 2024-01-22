using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PosBooksConsumer.Models;

namespace PosBooksConsumerTests.Integracao 
{
    public class Setup : IDisposable
    {
        public readonly PBCContext _context;

        public Setup()
        {
            _context = new PBCContext(GetContextOptions());
            _context.Database.Migrate();
        }

        private DbContextOptions<PBCContext> GetContextOptions()
        {
            var connection = "Server=localhost,1433;Database=PosBooksConsumer;User Id=sa;Password=posServer2024!;TrustServerCertificate=True;";

            return new DbContextOptionsBuilder<PBCContext>()
                .UseSqlServer(connection)
                .EnableSensitiveDataLogging()
                .Options;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
