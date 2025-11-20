using BookLendingService.Models;

namespace BookLendingService.Repositories;
public interface IBookRepository
{
    Task<Book> AddBookAsync(Book book);
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task UpdateBookAsync(Book book);
}
