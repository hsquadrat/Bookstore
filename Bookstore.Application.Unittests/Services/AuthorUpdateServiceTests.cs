﻿using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Services;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.Application.Unittests.Services
{
    public class AuthorUpdateServiceTests
    {
        private AuthorUpdateValidator Validator { get; }
        private IMapper Mapper { get; }

        public AuthorUpdateServiceTests()
        {
            Mapper = new MapperConfiguration(cfg =>
                cfg.AddMaps(typeof(DtoEntityMapperProfile))).CreateMapper();
            Validator = new AuthorUpdateValidator();
        }

        [Fact]
        public async Task Update_Existing_Author()
        {
            //Arrange
            var authorUpdate = new AuthorUpdate(1, "Test", "Test");
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(mock => mock.GetAuthorById(1))
                .ReturnsAsync(new Author());
            var authorUpdateService = new AuthorUpdateService(authorRepositoryMock.Object,
                Mapper, Validator);

            //Act
            await authorUpdateService.UpdateAuthor(authorUpdate);

            //
            authorRepositoryMock.Verify(mock => mock.Update(), Times.Once);
        }

        [Fact]
        public void AuthorNotFoundException_For_Non_Existent_Id()
        {
            //Arrange
            var authorUpdate = new AuthorUpdate(1, "Test", "Test");
            var authorRepositoryMock = new Mock<IAuthorRepository>();
            authorRepositoryMock.Setup(mock => mock.GetAuthorById(1))
                .Returns<Author?>(null);
            var authorUpdateService = new AuthorUpdateService(authorRepositoryMock.Object,
                Mapper, Validator);

            //Act
            Func<Task> func = async () => await authorUpdateService
                .UpdateAuthor(authorUpdate);

            //Assert
            Assert.ThrowsAsync<AuthorNotFoundException>(func);
        }
    }
}