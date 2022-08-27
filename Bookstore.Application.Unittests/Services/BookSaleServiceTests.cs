using AutoMapper;
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
    public class BookSaleServiceTests
    {
        private IMapper Mapper { get; }
        private BookSaleValidator BookSaleValidator { get; }
        private BookValidator BookValidator { get; }

        public BookSaleServiceTests()
        {
            Mapper = new MapperConfiguration(cfg =>
                cfg.AddMaps(typeof(DtoEntityMapperProfile))).CreateMapper();
            BookSaleValidator = new BookSaleValidator();
            BookValidator = new BookValidator();
        }

        [Fact]
        public async Task Quantity_Updated()
        {
            //Arrange
            var bookSale = new BookSale(1, 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookById(1))
                .ReturnsAsync(new Book() { Quantity = 1, Author = new Author() });
            var bookSaleService = new BookSaleService(bookRepositoryMock.Object,
                BookSaleValidator, BookValidator);

            //Act
            await bookSaleService.ProcessBookSale(bookSale);

            //Assert
            bookRepositoryMock.Verify(mock => mock.Update(), Times.Once);
        }

        [Fact]
        public void BookNotFoundException_For_Non_Existent_BookId()
        {
            //Arrange
            var bookSale = new BookSale(1, 1);
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(mock => mock.GetBookById(1))
                .Returns<Book?>(null);
            var bookSaleService = new BookSaleService(bookRepositoryMock.Object,
                BookSaleValidator, BookValidator);

            //Act
            Func<Task> func = async () => await
                bookSaleService.ProcessBookSale(bookSale);

            //Assert
            Assert.ThrowsAsync<BookNotFoundException>(func);
        }
    }
}