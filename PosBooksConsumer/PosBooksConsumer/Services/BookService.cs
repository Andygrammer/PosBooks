﻿using Microsoft.EntityFrameworkCore;
using PosBooksConsumer.Models;

namespace PosBooksConsumer.Services
{
    public interface IBookService
    {
        public Task<Book?> VerifyBookAvaliability(BookDTO book);

        public Task Rent(Book book, Client renter);
    }
    
    public class BookService : IBookService
    {
        private readonly PBCContext _context;

        public BookService(PBCContext context)
        {
            _context = context;
        }

        public async Task<Book?> VerifyBookAvaliability(BookDTO book)
        {
            var selectedBook = await _context.Books.Where(b =>
                b.Publisher == book.Publisher
                && b.Title == book.Title
                && b.Year == book.Year
                && b.Author == book.Author

            ).FirstOrDefaultAsync();

            if (selectedBook?.Renter != null) return null;
            return selectedBook;
        }

        public async Task Rent(Book book, Client renter)
        {
            book.Renter = renter;
            await _context.SaveChangesAsync();
        }
    }
}
