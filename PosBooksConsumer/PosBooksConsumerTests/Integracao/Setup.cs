using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PosBooksConsumer.Models;

namespace PosBooksConsumerTests.Integracao
{
    public class Setup : IDisposable
    {
        private readonly IServiceProvider _services;
        public readonly PBCContext _context;
        private SqliteConnection _connection;

        public Setup()
        {
            _context = new PBCContext(GetContextOptions());
            _context.Database.EnsureCreated();
        }

        private DbContextOptions<PBCContext> GetContextOptions()
        {
            var connection = "Server=(localdb)\\MSSQLLocalDB;Database=PosBooksConsumerTests;Integrated Security=true;";

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
