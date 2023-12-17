using System;
using System.Threading.Tasks;
using Acme.BookStore.Books;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using static Acme.BookStore.HttpApiConsts;

namespace Acme.BookStore.Controllers;

[Produces("application/json")]
[Route($"{RouteBook}")]
[ApiExplorerSettings(GroupName = "Books")]
public class BookController : BookStoreController
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookController(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [ApiExplorerSettings(GroupName = "Books")]
    [HttpPost($"send-image/id/{{bookId}}")]
    public async Task<Book> SendProfileImageAsync(Guid bookId) {
        var book = await _bookRepository.GetAsync(bookId);
        return book;
    }
}