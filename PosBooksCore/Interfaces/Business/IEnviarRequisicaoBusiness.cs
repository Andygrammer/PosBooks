using MassTransit;
using PosBooksCore.Dto;

namespace PosBooksCore.Interfaces.Business;

public interface IEnviarRequisicaoBusiness
{
    /// <summary>
    /// Sends a request using the provided <paramref name="solicitacaoDto"/> to the specified <paramref name="endpoint"/> asynchronously.
    /// </summary>
    /// <param name="solicitacaoDto">The DTO containing the request data.</param>
    /// <param name="endpoint">The ISendEndpoint instance to send the request to.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task EnviarRequisicao(SolicitacaoDto solicitacaoDto, ISendEndpoint endpoint);
}