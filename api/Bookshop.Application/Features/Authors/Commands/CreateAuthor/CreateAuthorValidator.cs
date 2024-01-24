using FluentValidation;

namespace Bookshop.Application.Features.Authors.Commands.CreateAuthor
{
    public class CreateAuthorValidator : AbstractValidator<CreateAuthor>
    {
        public CreateAuthorValidator()
        {
            RuleFor(p => p.Author.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Author.About)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
