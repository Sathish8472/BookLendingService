namespace BookLendingService.Models.DTOs;

public class BookResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }

    public BookResponse()
    {
    }

    public BookResponse(Book book)
    {
        Id = book.Id;
        Title = book.Title;
        Author = book.Author;
        IsAvailable = book.IsAvailable;
    }
}
