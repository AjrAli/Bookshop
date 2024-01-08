using FluentValidation;

namespace Bookshop.Application.Features.Customers.Commands.EditPassword
{
    public class EditPasswordValidator : AbstractValidator<EditPassword>
    {
        public EditPasswordValidator()
        {
            //UserData for Customer
            // Current Password
            RuleFor(p => p.Customer.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(4).WithMessage("{PropertyName} must have minimum 4 characters.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Customer.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Equal(p => p.Customer.Password).WithMessage("{PropertyName} must be the same as field password.")
                .MinimumLength(4).WithMessage("{PropertyName} must have minimum 4 characters.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            // New Password
            RuleFor(p => p.Customer.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(4).WithMessage("{PropertyName} must have minimum 4 characters.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Customer.ConfirmNewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Equal(p => p.Customer.NewPassword).WithMessage("{PropertyName} must be the same as field NewPassword.")
                .MinimumLength(4).WithMessage("{PropertyName} must have minimum 4 characters.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}
