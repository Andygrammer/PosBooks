using Microsoft.EntityFrameworkCore;
using PosBooksConsumer;
using PosBooksConsumer.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddDbContext<PBCContext>(opt => opt
            .UseSqlite("DataSource=:memory:")
            .EnableSensitiveDataLogging());
    })
    .Build();

var dbContext = host.Services.GetRequiredService<PBCContext>();
dbContext.Database.Migrate();

host.Run();

public partial class Program { }