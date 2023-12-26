using MassTransit;
using Microsoft.Extensions.Configuration;
using PosBooksCore.Dto;
using PosBooksCore.Interfaces.Parameters;

namespace PosBooksCore.Parameters;

/// <summary>
/// Represents a set of parameters for configuring a system
/// </summary>
public class Parametros : IParametros
{
    private readonly IConfiguration _configuration;
    private readonly IBus _bus;
    const string MASSTRANSIT = "MassTransit";
    const string NOMEFILA = "NomeFila";
    const string QUEUE = "queue";
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="bus"></param>
    public Parametros(
        IConfiguration configuration,
        IBus bus)
    {
        _configuration = configuration;
        _bus = bus;
    }

    /// <summary>
    /// Searches for the name of the queue.
    /// </summary>
    /// <returns>The name of the queue.</returns>
    public string BuscarNomeFila()
    {
        return _configuration.GetSection(MASSTRANSIT)[NOMEFILA] ?? string.Empty;
    }
    
    /// <summary>
    /// MontarEndpoint method is used to create an ISendEndpoint for the specified queue name.
    /// </summary>
    /// <param name="nomeFila">The name of the queue to create the endpoint for.</param>
    /// <returns>
    /// A Task representing the asynchronous operation. The task result is an ISendEndpoint which can be used to send messages to the specified queue.
    /// </returns>
    public async Task<ISendEndpoint> MontarEndpoint(string nomeFila)
    {
        return await _bus.GetSendEndpoint(new Uri($"{QUEUE}:{nomeFila}"));
    }
}