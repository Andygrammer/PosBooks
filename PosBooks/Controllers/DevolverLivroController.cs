using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PosBooksCore.Dto;
using PosBooksCore.Extensions;
using PosBooksCore.Interfaces.Business;
using PosBooksCore.Interfaces.Parameters;
using PosBooksCore.ViewModels;

namespace PosBooks.Controllers;

[ApiController]
[Route("v1/DevolverLivro/")]
public class DevolverLivroController : ControllerBase
{
    private readonly IParametros _parametros;
    private readonly IEnviarRequisicaoBusiness _enviarRequisicaoBusiness;
    const string NOMEFILADEVOLVERLIVRO = "NomeFilaDevolverLivro";
    
    /// <summary>
    /// Constructor
    /// </summary>
    public DevolverLivroController(
        IParametros parametros,
        IEnviarRequisicaoBusiness enviarRequisicaoBusiness)
    {
        _parametros = parametros;
        _enviarRequisicaoBusiness = enviarRequisicaoBusiness;
    }
    
    /// <summary>
    /// Devolve um livro com base nas informações fornecidas na solicitação.
    /// </summary>
    /// <param name="solicitacaoDto">Os dados da solicitação de devolução do livro.</param>
    /// <returns>O resultado da operação de devolutiva do livro.</returns>
    [HttpPost]
    public async Task<IActionResult> DevolverLivro([FromBody] SolicitacaoDto solicitacaoDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            await SendSolicitacao(solicitacaoDto);
            return Ok(new ResultViewModel<dynamic>(new
            {
                solicitacaoDto.Nome,
                solicitacaoDto.Email,
                solicitacaoDto.IdLivro,
                solicitacaoDto.DataSolicitacao
            }));
        }
        catch (RabbitMqAddressException n)
        {
            return StatusCode(400, new ResultViewModel<string>($"Erro ao enviar a solicitação: {n.Message}"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna do servidor!"));
        }
    }
    
    /// <summary>
    /// Sends a solicitation using RabbitMQ.
    /// </summary>
    /// <param name="solicitacaoDto">The solicitation object to send. </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="RabbitMqAddressException">Thrown when there is an error with the RabbitMQ address.</exception>
    /// <exception cref="Exception">Thrown when there is an unexpected error.</exception>
    private async Task SendSolicitacao(SolicitacaoDto solicitacaoDto)
    {
        var nomeFila = _parametros.BuscarNomeFila(NOMEFILADEVOLVERLIVRO);
        var endpoint = await _parametros.MontarEndpoint(nomeFila);
        await _enviarRequisicaoBusiness.EnviarRequisicao(solicitacaoDto, endpoint);
    }
}