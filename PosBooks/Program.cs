using MassTransit;
using Microsoft.EntityFrameworkCore;
using PosBooks.Model;
using PosBooks.Services;
using PosBooksCore.Business;
using PosBooksCore.Interfaces.Business;
using PosBooksCore.Interfaces.Parameters;
using PosBooksCore.Parameters;

const string MASSTRANSIT = "MassTransit";
const string SERVIDOR = "Servidor";
const string USUARIO = "Usuario";
const string SENHA = "Senha";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IParametros, Parametros>();
builder.Services.AddSingleton<IEnviarRequisicaoBusiness, EnviarRequisicaoBusiness>();

builder.Services.AddDbContext<PosBookContext>(opt => opt
                                  .UseSqlite("DataSource=memory.db;Cache=Shared")
                                  .EnableSensitiveDataLogging());

builder.Services.AddScoped<PosBookContext>();
builder.Services.AddScoped<IBookService, BookService>();

ConfigureMassTransit(builder);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

void ConfigureMassTransit(WebApplicationBuilder webApplicationBuilder)
{
    var config = webApplicationBuilder.Configuration;
    var servidor = config.GetSection(MASSTRANSIT)[SERVIDOR]?? string.Empty;
    var user = config.GetSection(MASSTRANSIT)[USUARIO]?? string.Empty;
    var pass = config.GetSection(MASSTRANSIT)[SENHA]?? string.Empty;

    webApplicationBuilder.Services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(servidor, "/", h =>
            {
                h.Username(user);
                h.Password(pass);
            });
            
            cfg.ConfigureEndpoints(context);
        });
    });
}