using Bookshop.Application.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Bookshop.Application.Features.Customers.Helpers
{
    public static class UserCreationExceptionHelper
    {
        public static void ThrowUserCreationBadRequestException(IEnumerable<IdentityError> errors, string userName)
        {
            var errorDescriptions = errors.Select(error => error.Description);
            var errorMessage = $"Failed to create user '{userName}'. Errors: {string.Join(", ", errorDescriptions)}";
            throw new BadRequestException(errorMessage);
        }
    }
}
