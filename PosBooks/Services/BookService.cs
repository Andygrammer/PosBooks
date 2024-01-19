using Microsoft.EntityFrameworkCore;
using PosBooks.Model;
using PosBooksCore.Models;

namespace PosBooks.Services;

public interface IBookService
{
    Task<Book?> GetBook(int id);
    Task<IEnumerable<Book?>> ListBooks();
}
public class BookService : IBookService
{
    private readonly PosBookContext _context;

    public BookService(PosBookContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetBook(int id)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book?>> ListBooks()
    {
        return await _context.Books.ToListAsync();
    }
}

