using MassTransit;
using PosBooksConsumer.Services;
using PosBooksCore.Models;

namespace PosBooksConsumer.Events
{
    public  class GiveBackBook : IConsumer<BookRequest>
    {
        private readonly IBookService _bookService;
        private readonly IEmailService _emailService;

        public GiveBackBook(IBookService bookService, IEmailService emailService)
        {
            _bookService = bookService;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<BookRequest> context)
        {
            var bookId = context.Message.IdBook;
            await _bookService.GiveBackBook(bookId);
        }

        public async Task GiveBack(int idBook, Client renter)
        {
            var book = await _bookService.VerifyBookAvaliability(idBook);
            if (book.Renter == renter)
            {
                await _bookService.GiveBackBook(idBook);
                await _emailService.SendEmail(renter.Email, "Livro Devolvido", $"O livro {book.Title}, do(a) autor(a) {book.Author}, foi devolvido!");

                var nextCLient = await _bookService.NextInTheQeue(idBook);
                if (nextCLient != null) await _emailService.SendEmail(renter.Email, "Livro Disponível", $"O livro {book.Title}, do(a) autor(a) {book.Author}, foi devolvido e está disponível!");
            }          
        }
    }
}
