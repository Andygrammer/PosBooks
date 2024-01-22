using Microsoft.EntityFrameworkCore;
using PosBooksConsumer.Models;
using PosBooksCore.Models;

namespace PosBooksConsumer.Services
{
    public interface IBookService
    {
        public Task<Book?> VerifyBookAvaliability(int idBook);
        public Task Rent(int idBook, Client renter);
        public Task SubscribeToWaitList(int idBook, Client renter);
        public Task GiveBackBook(int id);
    }
    
    public class BookService : IBookService
    {
        private readonly PBCContext _context;

        public BookService(PBCContext context)
        {
            _context = context;
        }

        public async Task<Book?> VerifyBookAvaliability(int idBook)
        {
            var selectedBook = await _context.Books
                .Where(b =>b.Id == idBook)
                .Include(c => c.Renter)
                .FirstOrDefaultAsync();

            return selectedBook;
        }

        public async Task<bool> VerifyIfClientExists(string email) =>
            await _context.Clients
                .Select(b => b.Email == email)
                .FirstOrDefaultAsync();

        public async Task Rent(int idBook, Client renter)
        {
            var wantedBook = await _context.Books.Where(b => b.Id == idBook).FirstAsync();
            wantedBook.Renter = renter;
            await _context.SaveChangesAsync();
        }

        public async Task GiveBackBook(int id)
        {
            var selectedBook = await _context.Books.Where(b => b.Id == id).FirstOrDefaultAsync();
            selectedBook.Renter = null;
            await _context.SaveChangesAsync();
        }

        public async Task SubscribeToWaitList(int idBook, Client renter)
        {
            var wantedBook = await _context.Books.Where(b => b.Id == idBook).FirstAsync();

            var waitListNewRequest = new WaitList() 
            { BookRequest = wantedBook, RequestDate = DateTime.Now, Requester = renter };

            await _context.WaitList.AddAsync(waitListNewRequest);
            await _context.SaveChangesAsync();
        }
    }
}
