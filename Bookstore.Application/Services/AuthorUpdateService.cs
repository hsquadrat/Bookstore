using AutoMapper;
using Bookstore.Application.Contracts;
using Bookstore.Application.Dtos;
using Bookstore.Application.Exceptions;
using Bookstore.Application.Validation;
using Bookstore.Domain.Entities;
using FluentValidation;

namespace Bookstore.Application.Services
{
    public class AuthorUpdateService
    {
        public IAuthorRepository AuthorRepository { get; }
        public IMapper Mapper { get; }
        public AuthorUpdateValidator AuthorUpdateValidator { get; }

        public AuthorUpdateService(IAuthorRepository authorRepository, IMapper mapper,
            AuthorUpdateValidator authorUpdateValidator)
        {
            AuthorRepository = authorRepository;
            Mapper = mapper;
            AuthorUpdateValidator = authorUpdateValidator;
        }

        public async Task UpdateAuthor(AuthorUpdate authorUpdate)
        {
            await AuthorUpdateValidator.ValidateAndThrowAsync(authorUpdate);
            Author? author = await AuthorRepository.GetAuthorById(authorUpdate.AuthorId);

            if (author == null)
                throw new AuthorNotFoundException();

            Mapper.Map(authorUpdate, author);
            await AuthorRepository.Update();
        }
    }
}