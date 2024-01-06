using MassTransit;
using Microsoft.EntityFrameworkCore;
using PosBooksConsumer;
using PosBooksConsumer.Events;
using PosBooksConsumer.Models;
using PosBooksConsumer.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddDbContext<PBCContext>(opt => opt
            .UseSqlite("DataSource=:memory:")
            .EnableSensitiveDataLogging());

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IEmailService, EmailService>();

        var configuration = hostContext.Configuration;
        var fila = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;
        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });
                cfg.ReceiveEndpoint(fila, e =>
                {
                    e.Consumer<RentBook>(context);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<RentBook>();
        });
    })
    .Build();

var dbContext = host.Services.GetRequiredService<PBCContext>();
dbContext.Database.Migrate();

host.Run();

public partial class Program { }