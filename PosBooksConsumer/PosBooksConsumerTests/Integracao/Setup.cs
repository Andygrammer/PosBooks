using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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
            _context = new PBCContext(GetInMemoryConnectionOptions());
            _context.Database.EnsureCreated();

            if (_connection is not null) _connection.Open();
        }

        private DbContextOptions<PBCContext> GetInMemoryConnectionOptions()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            return new DbContextOptionsBuilder<PBCContext>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
