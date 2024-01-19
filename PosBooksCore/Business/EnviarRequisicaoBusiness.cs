using MassTransit;
using PosBooksCore.Dto;
using PosBooksCore.Interfaces.Business;
using PosBooksCore.Models;

namespace PosBooksCore.Business;

public class EnviarRequisicaoBusiness : IEnviarRequisicaoBusiness
{
    /// <summary>
    /// Sends a request using the provided <paramref name="solicitacaoDto"/> to the specified <paramref name="endpoint"/> asynchronously.
    /// </summary>
    /// <param name="solicitacaoDto">The DTO containing the request data.</param>
    /// <param name="endpoint">The ISendEndpoint instance to send the request to.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task EnviarRequisicao(BookRequest solicitacaoDto, ISendEndpoint endpoint)
    {
        await endpoint.Send(solicitacaoDto);
    }
}