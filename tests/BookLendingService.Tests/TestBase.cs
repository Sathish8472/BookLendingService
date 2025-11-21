using BookLendingService.Models;

namespace BookLendingService.Tests;

public abstract class TestBase
{
    protected static Book CreateBook(
        int id = 1,
        string title = "Sample Book",
        bool isAvailable = true)
    {
        return new Book
        {
            Id = id,
            Title = title,
            Author = "Author",
            IsAvailable = isAvailable
        };
    }
}