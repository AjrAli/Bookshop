using FluentValidation;

namespace Bookshop.Application.Features.Books.Commands.Comments.AddComment
{
    public class AddCommentValidator : AbstractValidator<AddComment>
    {
        public AddCommentValidator()
        {
            RuleFor(p => p.Comment.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Comment.Content)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(1000).WithMessage("{PropertyName} must not exceed 1000 characters.");
            RuleFor(p => p.Comment.Rating)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(5).WithMessage("{PropertyName} must be less or equal to 5");
            RuleFor(p => p.Comment.BookId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} not found");
        }
    }
}
