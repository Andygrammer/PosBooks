using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PosBooksCore.Dto;

/// <summary>
/// Represents a data transfer object for a solicitation.
/// </summary>
public class SolicitacaoDto
{
    /// <summary>
    /// Name of the user.
    /// </summary>
    [Required(ErrorMessage = "Nome obrigatório")]
    public string Nome { get; set; }
    /// <summary>
    /// Email of the user.
    /// </summary>
    [Required(ErrorMessage = "E-mail obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    /// <summary>
    /// Id of the book.
    /// </summary>
    [Required(ErrorMessage = "ID do Livro obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "IdLivro deve ser maior que 0.")]
    public int IdLivro { get; set; }
    /// <summary>
    /// Date of the solicitation.
    /// </summary>
    [JsonIgnore]
    public DateTime DataSolicitacao { get;}

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="nome"></param>
    /// <param name="email"></param>
    /// <param name="idLivro"></param>
    public SolicitacaoDto(string nome, string email, int idLivro)
    {
        Nome = nome;
        Email = email;
        IdLivro = idLivro;
        DataSolicitacao = DateTime.Now;
    }
}