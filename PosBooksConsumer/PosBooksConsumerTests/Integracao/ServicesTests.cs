using Microsoft.EntityFrameworkCore;
using PosBooksConsumer.Models;
using PosBooksConsumer.Services;
using PosBooksConsumerTests.Shareable;

namespace PosBooksConsumerTests.Integracao
{
    public class ServicesTests : IClassFixture<Setup>
    {
        private readonly BookService _bookService;
        private readonly PBCContext _context;
        private readonly Setup _base;

        public ServicesTests(Setup testeBase)
        {
            _base = testeBase;
            _context = testeBase._context;
            _bookService = new BookService(_context);
        }

        [Fact]
        public async Task BookAvaliability_ShouldReturnObjectWhenItsAvaliable()
        {
            var bookRequest = new BookBuilder().Generate();

            await _context.AddAsync(bookRequest);
            await _context.SaveChangesAsync();

            var avaliabilityResult = await _bookService.VerifyBookAvaliability(bookRequest);
            Assert.Equal(bookRequest, avaliabilityResult);
        }

        [Fact]
        public async Task BookAvaliability_ShouldNotReturnObjectWhenItsUnavaliable()
        {
            var bookRequest = new BookBuilder().Generate();

            var avaliabilityResult = await _bookService.VerifyBookAvaliability(bookRequest);
            Assert.Null(avaliabilityResult);
        }

        [Fact]
        public async Task Rent_ShouldSaveWithRequester()
        {
            var bookRequest = new BookBuilder().Generate();
            var clientRequest = new ClientBuilder().Generate();

            await _context.AddAsync(bookRequest);
            await _context.SaveChangesAsync();

            var bookSaved = await _context.Books.Where(x => x.Title == bookRequest.Title).FirstAsync();

            await _bookService.Rent(bookSaved, clientRequest);

            var bookExpected = await _context.Books.Where(x => x.Title == bookRequest.Title).FirstAsync();
            Assert.Equal(clientRequest,  bookExpected.Renter);
        }
    }
}
