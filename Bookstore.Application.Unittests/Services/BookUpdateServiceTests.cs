﻿using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Services;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Validation;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.Application.Unittests.Services
{
    public class BookUpdateServiceTests
    {
        private IMapper Mapper { get; }
        private BookUpdateValidator BookUpdateValidator { get; }
        private BookValidator BookValidator { get; }

        public BookUpdateServiceTests()
        {
            Mapper = new MapperConfiguration(cfg =>
                cfg.AddMaps(typeof(DtoEntityMapperProfile))).CreateMapper();
            BookValidator = new BookValidator();
            BookUpdateValidator = new BookUpdateValidator();
        }

        [Fact]
        public async Task Update_Book()
        {
            //Arrange
            var bookUpdate = new BookUpdate(1, "1234567891234", "Test", 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookById(1))
                .ReturnsAsync(new Book());
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(mock => mock.GetAuthorById(1))
                .ReturnsAsync(new Author());
            var bookUpdateService = new BookUpdateService(bookRepositoryMock.Object,
                authorRepositoryMock.Object, Mapper, BookValidator, BookUpdateValidator);

            //Act
            await bookUpdateService.UpdateBook(bookUpdate);

            //Assert
            bookRepositoryMock.Verify(mock => mock.Update(), Times.Once);
        }

        [Fact]
        public void BookNotFoundException_For_Non_Existent_Book()
        {
            //Arrange
            var bookUpdate = new BookUpdate(1, "1234567891234", "Test", 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookById(1))
                .Returns<Book?>(null);
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(mock => mock.GetAuthorById(1))
                .ReturnsAsync(new Author());
            var bookUpdateService = new BookUpdateService(bookRepositoryMock.Object,
                authorRepositoryMock.Object, Mapper, BookValidator, BookUpdateValidator);

            //Act
            Func<Task> func = async () =>
                await bookUpdateService.UpdateBook(bookUpdate);

            //Assert
            Assert.ThrowsAsync<BookNotFoundException>(func);
        }

        [Fact]
        public void AuthorNotFoundException_For_Non_Existent_Author()
        {
            //Arrange
            var bookUpdate = new BookUpdate(1, "1234567891234", "Test", 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookById(1))
                .ReturnsAsync(new Book());
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(mock => mock.GetAuthorById(1))
                .Returns<Author?>(null);
            var bookUpdateService = new BookUpdateService(bookRepositoryMock.Object,
                authorRepositoryMock.Object, Mapper, BookValidator, BookUpdateValidator);

            //Act
            Func<Task> func = async () =>
                await bookUpdateService.UpdateBook(bookUpdate);

            //Assert
            Assert.ThrowsAsync<AuthorNotFoundException>(func);
        }

        [Fact]
        public void BookForIsbnDuplicateException_For_Duplicate_Isbn()
        {
            //Arrange
            var bookUpdate = new BookUpdate(1, "1234567891234", "Test", 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookById(1))
                .ReturnsAsync(new Book() { Id = 1 });
            bookRepositoryMock.Setup(mock => mock.GetBookByIsbn("1234567891234"))
                .ReturnsAsync(new Book() { Id = 2 });
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(mock => mock.GetAuthorById(1))
                .ReturnsAsync(new Author());
            var bookUpdateService = new BookUpdateService(bookRepositoryMock.Object,
                authorRepositoryMock.Object, Mapper, BookValidator, BookUpdateValidator);

            //Act
            Func<Task> func = async () =>
                await bookUpdateService.UpdateBook(bookUpdate);

            //Assert
            Assert.ThrowsAsync<BookForIsbnDuplicateException>(func);
        }
    }
}