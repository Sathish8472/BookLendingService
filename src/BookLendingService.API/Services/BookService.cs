using BookLendingService.Common;
using BookLendingService.Models;
using BookLendingService.Models.DTOs;
using BookLendingService.Repositories;

namespace BookLendingService.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task<ApiResponse<BookResponse>> AddBookAsync(CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            IsAvailable = true
        };

        var addedBook = await _bookRepository.AddBookAsync(book);

        return new ApiResponse<BookResponse>(true, "Book added successfully", new BookResponse(addedBook));
    }

    public async Task<ApiResponse<List<BookResponse>>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllBooksAsync();

        var bookDtos = books.Select(b => new BookResponse(b)).ToList();

        return new ApiResponse<List<BookResponse>>(true, "Books retrieved successfully", bookDtos);
    }

    public async Task<ApiResponse<BookResponse>> CheckOutBookAsync(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);

        if (book == null)
            return new ApiResponse<BookResponse>(false, "Book not found");

        if (!book.IsAvailable)
            return new ApiResponse<BookResponse>(false, "Book is already checked out");

        book.IsAvailable = false;
        await _bookRepository.UpdateBookAsync(book);

        return new ApiResponse<BookResponse>(true, "Book checked out successfully", new BookResponse(book));
    }

    public async Task<ApiResponse<BookResponse>> ReturnBookAsync(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);

        if (book == null)
            return new ApiResponse<BookResponse>(false, "Book not found");

        if (book.IsAvailable)
            return new ApiResponse<BookResponse>(false, "Book is already available");

        book.IsAvailable = true;
        await _bookRepository.UpdateBookAsync(book);

        return new ApiResponse<BookResponse>(true, "Book returned successfully", new BookResponse(book));
    }

}
