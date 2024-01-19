using PosBooksCore.Dto;

namespace PosBooksCore.Models
{
    public class BookRequest
    {
        public Client Requester { get; set; }

        public int IdBook { get; set; }

        public static BookRequest Map(SolicitacaoDto requestDto) =>
            new BookRequest()
            {
                IdBook = requestDto.IdLivro,
                Requester = new Client()
                {
                    Email = requestDto.Email,
                    Name = requestDto.Nome
                }
            };
    }
}
