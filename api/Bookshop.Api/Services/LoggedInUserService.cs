using Bookshop.Persistence.Contracts;
using System.Security.Claims;

namespace Bookshop.Api.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string? UserId { get; set; }
    }
}
