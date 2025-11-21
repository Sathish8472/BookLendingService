using BookLendingService.Common;
using BookLendingService.Controllers;
using BookLendingService.Models.DTOs;
using BookLendingService.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookLendingService.Tests.Unit;

public class BooksControllerTests : TestBase
{
    private readonly Mock<IBookService> _mockService;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _mockService = new Mock<IBookService>();
        _controller = new BooksController(_mockService.Object);
    }

    [Fact]
    public async Task AddBook_ShouldReturnOk_WhenSuccess()
    {
        var dto = new CreateBookDto { Title = "Test", Author = "A" };
        var response = new ApiResponse<BookResponse>(true, "ok", new BookResponse(CreateBook()));

        _mockService.Setup(s => s.AddBookAsync(It.IsAny<CreateBookDto>()))
                     .ReturnsAsync(response);

        var result = await _controller.AddBook(dto) as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task AddBook_ShouldReturnBadRequest_WhenFail()
    {
        var dto = new CreateBookDto();
        _mockService.Setup(s => s.AddBookAsync(It.IsAny<CreateBookDto>()))
                    .ReturnsAsync(new ApiResponse<BookResponse>(false, "fail"));

        var result = await _controller.AddBook(dto) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetBooks_ShouldReturnOk_WhenSuccess()
    {
        var response = new ApiResponse<List<BookResponse>>(true, "ok",
            new List<BookResponse> { new BookResponse(CreateBook()) });

        _mockService.Setup(s => s.GetAllBooksAsync()).ReturnsAsync(response);

        var result = await _controller.GetBooks() as OkObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetBooks_ShouldReturnBadRequest_WhenFail()
    {
        _mockService.Setup(s => s.GetAllBooksAsync())
                    .ReturnsAsync(new ApiResponse<List<BookResponse>>(false, "fail"));

        var result = await _controller.GetBooks() as BadRequestObjectResult;

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CheckOut_ShouldReturnOk_WhenSuccess()
    {
        var response = new ApiResponse<BookResponse>(true, "ok", new BookResponse(CreateBook()));

        _mockService.Setup(s => s.CheckOutBookAsync(1))
                    .ReturnsAsync(response);

        var result = await _controller.CheckOut(1) as OkObjectResult;

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CheckOut_ShouldReturnBadRequest_WhenFail()
    {
        _mockService.Setup(s => s.CheckOutBookAsync(1))
                    .ReturnsAsync(new ApiResponse<BookResponse>(false, "fail"));

        var result = await _controller.CheckOut(1) as BadRequestObjectResult;

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Return_ShouldReturnOk_WhenSuccess()
    {
        var response = new ApiResponse<BookResponse>(true, "ok", new BookResponse(CreateBook()));

        _mockService.Setup(s => s.ReturnBookAsync(1))
                    .ReturnsAsync(response);

        var result = await _controller.Return(1) as OkObjectResult;

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Return_ShouldReturnBadRequest_WhenFail()
    {
        _mockService.Setup(s => s.ReturnBookAsync(1))
                    .ReturnsAsync(new ApiResponse<BookResponse>(false, "fail"));

        var result = await _controller.Return(1) as BadRequestObjectResult;

        result.Should().NotBeNull();
    }
}
