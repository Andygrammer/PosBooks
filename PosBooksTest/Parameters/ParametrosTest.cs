using Microsoft.Extensions.Configuration;
using MassTransit;
using NSubstitute;
using PosBooksCore.Interfaces.Parameters;
using PosBooksCore.Parameters;

namespace PosBooksTest.Parameters;

public class ParametrosTest
{
    private readonly Parametros _parametros;
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;
    const string MASSTRANSIT = "MassTransit";
    const string NOMEFILA = "NomeFila";
    const string QUEUE = "queue";
    const string NOMEFILAALUGARLIVRO = "NomeFilaAlugarLivro";

    public ParametrosTest()
    {
        _bus = Substitute.For<IBus>();
        _configuration = Substitute.For<IConfiguration>();
        _parametros = new Parametros(_configuration, _bus);
    }

    [Fact]
    public async Task MontarEndpoint_ValidNomeFila_ReturnsSendEndpoint()
    {
        // Arrange
        var uri = new Uri($"{QUEUE}:{NOMEFILA}");
        _bus.GetSendEndpoint(uri).Returns(Substitute.For<ISendEndpoint>());

        // Act
        var result = await _parametros.MontarEndpoint(NOMEFILA);

        // Assert
        Assert.IsAssignableFrom<ISendEndpoint>(result);
        await _bus.Received().GetSendEndpoint(uri);
    }
    
    [Fact]
    public void BuscarNomeFila_ConfigurationHasKey_ReturnsNomeFila()
    {
        // Arrange
        _configuration.GetSection(MASSTRANSIT)[NOMEFILA].Returns(NOMEFILA);

        // Act
        var result = _parametros.BuscarNomeFila(NOMEFILAALUGARLIVRO);

        // Assert
        Assert.Equal(NOMEFILA, result);
    }

    [Fact]
    public void BuscarNomeFila_ConfigurationDoesNotHaveKey_ReturnsEmptyString()
    {
        // Arrange
        _configuration.GetSection(MASSTRANSIT)[NOMEFILA].Returns((string)null);

        // Act
        var result = _parametros.BuscarNomeFila(NOMEFILAALUGARLIVRO);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}