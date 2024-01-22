using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PosBooksConsumer.Events;
using PosBooksConsumer.Models;
using PosBooksConsumer.Services;
using PosBooksConsumerTests.Shareable;
using PosBooksCore.Models;

namespace PosBooksConsumerTests.Integracao 
{
    [Collection("ConsumerTests")]
    public class EventsTests : IClassFixture<Setup>
    {
        private readonly IBookService _bookService;
        private readonly IBookService _bookServiceStub;
        private readonly IEmailService _emailService;
        private readonly RentBook _rentBookWithStub;
        private readonly RentBook _rentBook;
        private readonly PBCContext _context;

        public EventsTests(Setup setup)
        {
            _bookService = new BookService(setup._context);
            _emailService = Substitute.For<IEmailService>();
            _bookServiceStub = Substitute.For<IBookService>();
            _rentBookWithStub = new RentBook(_bookServiceStub, _emailService);
            _rentBook = new RentBook(_bookService, _emailService);
            _context = setup._context;
        }

        [Fact]
        public async Task Rent_ShouldNotCallRentService_WhenTryingToRentToCurrentRenter()
        {
            var bookId = 1;
            var currentRenter = new ClientBuilder().Generate();
            var book = new BookBuilder().WithId(bookId).WithRenter(currentRenter);

            _bookServiceStub.VerifyBookAvaliability(Arg.Any<int>()).Returns(book);

            await _bookService.Rent(bookId, currentRenter);

            await _rentBookWithStub.Rent(new BookRequest() { IdBook = bookId, Requester = currentRenter });

            await _bookServiceStub.Received(0).Rent(Arg.Any<int>(), Arg.Any<Client>());
        }
        
        [Fact]
        public async Task Rent_ShouldNotCallSubscribeToWaitListService_WhenTryingToRentToCurrentRenter()
        {
            var bookId = 1;
            var currentRenter = new ClientBuilder().Generate();
            var book = new BookBuilder().WithId(bookId).WithRenter(currentRenter);

            _bookServiceStub.VerifyBookAvaliability(Arg.Any<int>()).Returns(book);
            await _bookService.Rent(bookId, currentRenter);

            await _rentBookWithStub.Rent(new BookRequest() { IdBook = bookId, Requester = currentRenter });

            await _bookServiceStub.Received(0).SubscribeToWaitList(Arg.Any<int>(), Arg.Any<Client>());
        }

        [Fact]
        public async Task Rent_ShouldNotRent_WhenBookIsAlreadyRented()
        {
            var bookId = 1;
            var currentRenter = new ClientBuilder().Generate();
            var newRenter = new ClientBuilder().Generate();

            await _bookService.Rent(bookId, currentRenter);

            await _rentBook.Rent(new BookRequest() { IdBook = bookId, Requester = newRenter });

            var finalBookState = await _bookService.VerifyBookAvaliability(bookId);

            Assert.Equal(finalBookState.Renter, currentRenter);
        }

        [Fact]
        public async Task Rent_ShouldSubscribeClientInWaitList_WhenBookIsAlreadyRented()
        {
            var bookId = 1;
            var currentRenter = new ClientBuilder().Generate();
            var newRenter = new ClientBuilder().Generate();

            await _bookService.Rent(bookId, currentRenter);

            await _rentBook.Rent(new BookRequest() { IdBook = bookId, Requester = newRenter });

            var waitListContent = await _context.WaitList
                .FirstOrDefaultAsync(x => x.BookRequest.Id == bookId && x.Requester.Email == newRenter.Email);

            Assert.NotNull(waitListContent);
        }

        [Fact]
        public async Task Rent_ShouldSendEmail_WhenBookIsRedyToBeRent()
        {
            var bookId = 1;
            var currentRenter = new ClientBuilder().Generate();
            var book = new BookBuilder().WithId(bookId).WithRenter(null);

            _bookServiceStub.VerifyBookAvaliability(Arg.Any<int>()).Returns(book);

            await _bookService.Rent(bookId, currentRenter);

            await _rentBookWithStub.Rent(new BookRequest() { IdBook = bookId, Requester = currentRenter });

            await _emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task Rent_ShouldSendEmail_WhenPutClientInTheWaitList()
        {
            var bookId = 1;
            var currentRenter = new ClientBuilder().Generate();
            var newRenter = new ClientBuilder().Generate();
            var book = new BookBuilder().WithId(bookId).WithRenter(currentRenter);

            _bookServiceStub.VerifyBookAvaliability(Arg.Any<int>()).Returns(book);

            await _bookService.Rent(bookId, currentRenter);

            await _rentBookWithStub.Rent(new BookRequest() { IdBook = bookId, Requester = newRenter });

            await _emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

    }
}
