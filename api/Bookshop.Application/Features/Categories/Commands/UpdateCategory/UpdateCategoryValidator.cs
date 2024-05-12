using FluentValidation;

namespace Bookshop.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategory>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0)
                .WithMessage("{PropertyName} not found");
            RuleFor(p => p.Category.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Category.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
