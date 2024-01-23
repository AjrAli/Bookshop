﻿using FluentValidation;
using static Bookshop.Domain.Entities.Book;

namespace Bookshop.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookValidator : AbstractValidator<CreateBook>
    {
        public CreateBookValidator()
        {
            RuleFor(p => p.Book.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Book.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Book.Publisher)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Book.Isbn)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Book.Price)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Book.Quantity)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Book.PageCount)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Book.Dimensions)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Book.Language)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .IsEnumName(typeof(Languages)).WithMessage("Invalid language value");
            RuleFor(p => p.Book.PublishDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(BeAValidDate)
                .WithMessage("Invalid date/time");
            RuleFor(p => p.Book.AuthorId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} not found");
            RuleFor(p => p.Book.CategoryId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} not found");
            RuleFor(p => p.Book.Image)
                .NotEmpty().WithMessage("{PropertyName} is required.");

        }
        private bool BeAValidDate(string value)
        {
            return DateTime.TryParse(value, out _);
        }
    }
}
