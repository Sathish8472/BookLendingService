using BookLendingService.Models.DTOs;
using BookLendingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLendingService.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(CreateBookDto dto)
    {
        var response = await _bookService.AddBookAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var response = await _bookService.GetAllBooksAsync();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("{id}/checkout")]
    public async Task<IActionResult> CheckOut(int id)
    {
        var response = await _bookService.CheckOutBookAsync(id);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("{id}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var response = await _bookService.ReturnBookAsync(id);
        return response.Success ? Ok(response) : BadRequest(response);
    }


}
