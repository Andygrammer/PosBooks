using MassTransit;
using PosBooksConsumer.Models;
using PosBooksConsumer.Services;

namespace PosBooksConsumer.Events
{
    public class RentBook : IConsumer<BookRequestDTO>
    {
        private readonly IBookService _bookService;
        private readonly IEmailService _emailService;

        public RentBook(IBookService bookService, IEmailService emailService)
        {
            _bookService = bookService;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<BookRequestDTO> context)
        {
            var avaliableBook = await _bookService.VerifyBookAvaliability(context.Message.Book);
            if (avaliableBook == null)
            {
                await _bookService.SubscribeToWaitList(context.Message.Book, context.Message.Requester);
                await _emailService.SendEmail(context.Message.Requester.Email, "");
            }

            await _bookService.Rent(context.Message.Book, context.Message.Requester);
            await _emailService.SendEmail(context.Message.Requester.Email, "");
        }
    }
}
