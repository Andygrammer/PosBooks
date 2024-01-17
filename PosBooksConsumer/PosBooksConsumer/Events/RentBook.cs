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

        public async Task Consume(ConsumeContext<BookRequest> context)
        {
            var avaliableBook = await _bookService.VerifyBookAvaliability(context.Message.IdBook);
            if (avaliableBook == null)
            {
                await _bookService.SubscribeToWaitList(context.Message.IdBook, context.Message.Requester);
                await _emailService.SendEmail(context.Message.Requester.Email, "");
            }

            await _bookService.Rent(context.Message.IdBook, context.Message.Requester);
            await _emailService.SendEmail(context.Message.Requester.Email, "");
        }
    }
}
