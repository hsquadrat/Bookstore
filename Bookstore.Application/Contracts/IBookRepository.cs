using Bookstore.Domain.Entities;

namespace Bookstore.Application.Contracts;

public interface IBookRepository
{
    Task<Book?> GetBookByIsbn(string isbn);
    Task<long> AddBook(Book book);
    Task<Book?> GetBookById(long isbn);
    Task Update();
}