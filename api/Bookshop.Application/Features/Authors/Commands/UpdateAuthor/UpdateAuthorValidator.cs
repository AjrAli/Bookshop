using FluentValidation;

namespace Bookshop.Application.Features.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorValidator : AbstractValidator<UpdateAuthor>
    {
        public UpdateAuthorValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} not found");
            RuleFor(p => p.Author.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Author.About)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
