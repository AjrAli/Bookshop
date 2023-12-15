using FluentValidation;

namespace Bookshop.Application.Features.Customers.Commands.EditCustomer
{
    public class EditCustomerValidator : AbstractValidator<EditCustomer>
    {
        public EditCustomerValidator()
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


            // Customer
            RuleFor(p => p.Customer.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.Customer.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");


            // BillingAddress
            RuleFor(p => p.Customer.BillingAddress.Street)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Customer.BillingAddress.PostalCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(10).WithMessage("{PropertyName} must not exceed 10 characters.");

            RuleFor(p => p.Customer.BillingAddress.State)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Customer.BillingAddress.City)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Customer.BillingAddress.Country)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            // ShippingAddress
            RuleFor(p => p.Customer.ShippingAddress.Street)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Customer.ShippingAddress.PostalCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(10).WithMessage("{PropertyName} must not exceed 10 characters.");

            RuleFor(p => p.Customer.ShippingAddress.State)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Customer.ShippingAddress.City)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Customer.ShippingAddress.Country)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}
