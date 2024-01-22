﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PosBooks.Controllers;
using PosBooksCore.Dto;
using PosBooksCore.Interfaces.Business;
using PosBooksCore.Interfaces.Parameters;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PosBooksCore.ViewModels;
using PosBooksCore.Models;

namespace PosBooksTest.Controllers;

public class AlugarLivroControllerTest
{
    private readonly AlugarLivroController _controller;
    private readonly IParametros _parametros = Substitute.For<IParametros>();
    private readonly IEnviarRequisicaoBusiness _enviarRequisicaoBusiness = Substitute.For<IEnviarRequisicaoBusiness>();
    const string NOMEFILAALUGARLIVRO = "NomeFilaAlugarLivro";

    public AlugarLivroControllerTest()
    {
        _controller = new AlugarLivroController(_parametros, _enviarRequisicaoBusiness);
    }
    
    [Fact]
    public void AlugarLivroTest_ShouldReturnSuccessWhenAluguelIsSuccessful()
    {
        // Arrange
        var solicitacaoDto = new SolicitacaoDto("Nome", "Email", 1);
    
        _parametros.BuscarNomeFila(NOMEFILAALUGARLIVRO).Returns("TestFila");
        var endpoint = Substitute.For<ISendEndpoint>();
        _parametros.MontarEndpoint("TestFila").Returns(endpoint);

        // Act
        var result = _controller.AlugarLivro(solicitacaoDto).Result;
        var requisicaoEsperada = BookRequest.Map(solicitacaoDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        _enviarRequisicaoBusiness.EnviarRequisicao(requisicaoEsperada, endpoint);
    }
    
    [Fact]
    public async Task AlugarLivroTest_ShouldReturnBadRequestWhenModelStateIsInvalid()
    {
        // Arrange
        var solicitacaoDto = new SolicitacaoDto("Nome", "Email", 1);
        _controller.ModelState.AddModelError("CampoInvalido", "Mensagem de erro");

        // Act
        var result = await _controller.AlugarLivro(solicitacaoDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task  AlugarLivro_RabbitMqAddressException_ReturnsBadRequest()
    {
        // Arrange
        var solicitacaoDto = new SolicitacaoDto("Nome", "Email", 1);
        _parametros.BuscarNomeFila(NOMEFILAALUGARLIVRO).Returns("InvalidFila"); 
        _parametros.MontarEndpoint("InvalidFila").Throws(new RabbitMqAddressException("Invalid address"));

        // Act
        var result = await  _controller.AlugarLivro(solicitacaoDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        var converter = Assert.IsType<ResultViewModel<string>>(statusCodeResult.Value);
        
        Assert.Equal(400, statusCodeResult.StatusCode);
        Assert.Equal("Erro ao enviar a solicitação: Invalid address", converter.Erros[0].ToString());
    }
    
    [Fact]
    public async Task AlugarLivro_GenericException_ReturnsInternalServerError()
    {
        // Arrange
        var solicitacaoDto = new SolicitacaoDto("Nome", "Email", 1);
        _parametros.BuscarNomeFila(NOMEFILAALUGARLIVRO).Returns("TestFila");
        _parametros.MontarEndpoint("TestFila").Returns(Substitute.For<ISendEndpoint>());
        var requisicaoEsperada = BookRequest.Map(solicitacaoDto);

        _enviarRequisicaoBusiness
            .EnviarRequisicao(requisicaoEsperada, Arg.Any<ISendEndpoint>())
            .ThrowsAsync(new Exception("Erro genérico"));
        
        // Act
        var result = await _controller.AlugarLivro(solicitacaoDto);
        
        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        var converter = Assert.IsType<ResultViewModel<string>>(statusCodeResult.Value);
        
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Falha interna do servidor!", converter.Erros[0].ToString());
    }
}