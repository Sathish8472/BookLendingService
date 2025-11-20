using BookLendingService.Data;
using BookLendingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLendingService.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _db;
    public BookRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<Book> AddBookAsync(Book book)
    {
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        return book;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _db.Books.ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task UpdateBookAsync(Book book)
    {
        _db.Books.Update(book);
        await _db.SaveChangesAsync();
    }
}
