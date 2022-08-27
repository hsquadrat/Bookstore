﻿using Bookstore.Application.Dtos;
using FluentValidation;

namespace Bookstore.Application.Validation;

public class AuthorUpdateValidator : AbstractValidator<AuthorUpdate>
{
    public AuthorUpdateValidator()
    {
        RuleFor(author => author.Firstname).NotEmpty().MaximumLength(50);
        RuleFor(author => author.Lastname).NotEmpty().MaximumLength(50);
    }
}