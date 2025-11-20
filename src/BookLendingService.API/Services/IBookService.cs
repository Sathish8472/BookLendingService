using BookLendingService.Common;
using BookLendingService.Models.DTOs;

namespace BookLendingService.Services;

public interface IBookService
{
    Task<ApiResponse<BookResponse>> AddBookAsync(CreateBookDto dto);
    Task<ApiResponse<List<BookResponse>>> GetAllBooksAsync();
    Task<ApiResponse<BookResponse>> CheckOutBookAsync(int id);
    Task<ApiResponse<BookResponse>> ReturnBookAsync(int id);
}
