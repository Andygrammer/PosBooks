using MassTransit;
using PosBooksCore.Dto;

namespace PosBooksCore.Interfaces.Parameters;

/// <summary>
/// Represents a set of parameters for interacting with a queue.
/// </summary>
public interface IParametros
{
    /// <summary>
    /// Searches for the name of the queue.
    /// </summary>
    /// <returns>The name of the queue.</returns>
    string BuscarNomeFila();

    /// <summary>
    /// MontarEndpoint method is used to create an ISendEndpoint for the specified queue name.
    /// </summary>
    /// <param name="nomeFila">The name of the queue to create the endpoint for.</param>
    /// <returns>
    /// A Task representing the asynchronous operation. The task result is an ISendEndpoint which can be used to send messages to the specified queue.
    /// </returns>
    Task<ISendEndpoint> MontarEndpoint(string nomeFila);
}