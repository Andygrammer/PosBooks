using MassTransit;
using PosBooksConsumer.Services;
using PosBooksCore.Models;

namespace PosBooksConsumer.Events
{
    public class RentBook : IConsumer<BookRequest>
    {
        private readonly IBookService _bookService;
        private readonly IEmailService _emailService;

        public RentBook(IBookService bookService, IEmailService emailService)
        {
            _bookService = bookService;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<BookRequest> context) =>
            Rent(new BookRequest() { Requester = context.Message.Requester, IdBook = context.Message.IdBook });

        public async Task Rent(BookRequest bookRequest)
        {
            var avaliableBook = await _bookService.VerifyBookAvaliability(bookRequest.IdBook);
            if (avaliableBook.Renter != null && avaliableBook.Renter.Email != bookRequest.Requester.Email)
            {
                await _bookService.SubscribeToWaitList(bookRequest.IdBook, bookRequest.Requester);
                await _emailService.SendEmail(bookRequest.Requester.Email, "Livro Indisponível", $"Lamentamos, mas o livro escolhido não está disponível no momento.");
            }
            else if(avaliableBook.Renter == null && avaliableBook.Renter.Email != bookRequest.Requester.Email)
            {
                await _bookService.Rent(bookRequest.IdBook, bookRequest.Requester);
                await _emailService.SendEmail(bookRequest.Requester.Email, "Livro Alugado", $"O livro {avaliableBook.Title}, do(a) autor(a) {avaliableBook.Author}, foi alugado!");
            }
        }
    }
}
