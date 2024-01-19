using MassTransit;
using PosBooksConsumer.Models;
using PosBooksConsumer.Services;
using PosBooksCore.Dto;
using PosBooksCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
