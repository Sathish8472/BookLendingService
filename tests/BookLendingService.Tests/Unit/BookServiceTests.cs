using BookLendingService.Common;
using BookLendingService.Models;
using BookLendingService.Models.DTOs;
using BookLendingService.Repositories;
using BookLendingService.Services;
using FluentAssertions;
using Moq;

namespace BookLendingService.Tests.Unit;

public class BookServiceTests : TestBase
{
    private readonly Mock<IBookRepository> _repoMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        _repoMock = new Mock<IBookRepository>();
        _service = new BookService(_repoMock.Object);
    }

    [Fact]
    public async Task AddBook_ShouldReturnSuccessResponse()
    {
        var dto = new CreateBookDto { Title = "Test", Author = "A" };
        _repoMock.Setup(r => r.AddBookAsync(It.IsAny<Book>()))
                 .ReturnsAsync((Book b) => { b.Id = 1; return b; });

        var response = await _service.AddBookAsync(dto);

        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Title.Should().Be("Test");
        response.Message.Should().Be("Book added successfully");
    }

    [Fact]
    public async Task GetAllBooks_ShouldReturnList()
    {
        _repoMock.Setup(r => r.GetAllBooksAsync())
                 .ReturnsAsync(new List<Book> { CreateBook() });

        var response = await _service.GetAllBooksAsync();

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task CheckOut_ShouldSucceed_WhenAvailable()
    {
        var book = CreateBook(isAvailable: true);
        _repoMock.Setup(r => r.GetBookByIdAsync(1)).ReturnsAsync(book);

        var result = await _service.CheckOutBookAsync(1);

        result.Success.Should().BeTrue();
        result.Data!.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public async Task CheckOut_ShouldFail_WhenAlreadyCheckedOut()
    {
        var book = CreateBook(isAvailable: false);
        _repoMock.Setup(r => r.GetBookByIdAsync(1)).ReturnsAsync(book);

        var result = await _service.CheckOutBookAsync(1);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Book is already checked out");
    }

    [Fact]
    public async Task CheckOut_ShouldFail_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetBookByIdAsync(1)).ReturnsAsync((Book?)null);

        var result = await _service.CheckOutBookAsync(1);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Book not found");
    }

    [Fact]
    public async Task Return_ShouldSucceed_WhenCheckedOut()
    {
        var book = CreateBook(isAvailable: false);
        _repoMock.Setup(r => r.GetBookByIdAsync(1)).ReturnsAsync(book);

        var response = await _service.ReturnBookAsync(1);

        response.Success.Should().BeTrue();
        response.Data!.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public async Task Return_ShouldFail_WhenAlreadyAvailable()
    {
        var book = CreateBook(isAvailable: true);
        _repoMock.Setup(r => r.GetBookByIdAsync(1)).ReturnsAsync(book);

        var result = await _service.ReturnBookAsync(1);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Book is already available");
    }

    [Fact]
    public async Task Return_ShouldFail_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetBookByIdAsync(1)).ReturnsAsync((Book?)null);

        var result = await _service.ReturnBookAsync(1);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Book not found");
    }
}
