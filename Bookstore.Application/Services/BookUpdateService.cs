using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Validation;
using FluentValidation;

namespace Bookstore.Application.Services
{
    public class BookUpdateService
    {
        public IBookRepository BookRepository { get; }
        public IAuthorRepository AuthorRepository { get; }
        public IMapper Mapper { get; }
        public BookValidator BookValidator { get; }
        public BookUpdateValidator BookUpdateValidator { get; }

        public BookUpdateService(IBookRepository bookRepository,
            IAuthorRepository authorRepository, IMapper mapper,
            BookValidator bookValidator,
            BookUpdateValidator bookUpdateValidator)
        {
            BookRepository = bookRepository;
            AuthorRepository = authorRepository;
            Mapper = mapper;
            BookValidator = bookValidator;
            BookUpdateValidator = bookUpdateValidator;
        }

        public async Task UpdateBook(BookUpdate bookUpdate)
        {
            await BookUpdateValidator.ValidateAndThrowAsync(bookUpdate);
            Book? book = await BookRepository.GetBookById(bookUpdate.BookId);
            Author? author = await AuthorRepository.GetAuthorById(bookUpdate.AuthorId);

            if (author == null)
                throw new AuthorNotFoundException();

            if (book == null)
                throw new BookNotFoundException();

            Book? existingBookForIsbn = await BookRepository.GetBookByIsbn(bookUpdate.Isbn);

            if (existingBookForIsbn != null && existingBookForIsbn.Id != book.Id)
                throw new BookForIsbnDuplicateException();

            book.Author = author;
            Mapper.Map(bookUpdate, book);
            await BookValidator.ValidateAndThrowAsync(book);
            await BookRepository.Update();
        }
    }
}