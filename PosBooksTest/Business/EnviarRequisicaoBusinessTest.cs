using MassTransit;
using NSubstitute;
using PosBooksCore.Business;
using PosBooksCore.Dto;

namespace PosBooksTest.Business;

public class EnviarRequisicaoBusinessTest
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly EnviarRequisicaoBusiness _enviarRequisicaoBusiness;
    private readonly SolicitacaoDto _solicitacaoDto;

    public EnviarRequisicaoBusinessTest()
    {
        // Arrange
        // Using NSubstitute to mock dependencies
        _sendEndpoint = Substitute.For<ISendEndpoint>();
        _enviarRequisicaoBusiness = new EnviarRequisicaoBusiness();
        _solicitacaoDto = new SolicitacaoDto("teste", "teste@teste.com.br", 1);
    }

    [Fact]
    public async Task EnviarRequisicao_SendEndpointCalled_WhenMethodCalled()
    {
        // Act
        // Call the method under test
        await _enviarRequisicaoBusiness.EnviarRequisicao(_solicitacaoDto, _sendEndpoint);

        // Assert
        // Check if the Send method of the endpoint was called with the correct request
        await _sendEndpoint.Received(1).Send(_solicitacaoDto);
    }
}