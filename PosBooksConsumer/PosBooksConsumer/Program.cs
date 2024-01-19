using MassTransit;
using Microsoft.EntityFrameworkCore;
using PosBooksConsumer;
using PosBooksConsumer.Events;
using PosBooksConsumer.Models;
using PosBooksConsumer.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();

        services.AddDbContext<PBCContext>(opt => opt
                                          .UseSqlServer(configuration.GetConnectionString("SQLConnection"))
                                          .EnableSensitiveDataLogging());

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<PBCContext>();

        
        var fila = configuration.GetSection("MassTransit")["NomeFilaAlugarLivro"] ?? string.Empty;
        var PosBooksProdutorDevolverLivro = configuration.GetSection("MassTransit")["PosBooksProdutorDevolverLivro"] ?? string.Empty;
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

                cfg.ReceiveEndpoint(PosBooksProdutorDevolverLivro, e =>
                {
                    e.Consumer<GiveBackBook>(context);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<RentBook>();
            x.AddConsumer<GiveBackBook>();
        });
    })
    .Build();

var scoped = host.Services.CreateScope();

var dbContext = scoped.ServiceProvider.GetRequiredService<PBCContext>();
dbContext.Database.Migrate();

host.Run();