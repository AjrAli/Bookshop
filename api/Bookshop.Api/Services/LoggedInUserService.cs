using Bookshop.Persistence.Contracts;
using System.Security.Claims;

namespace Bookshop.Api.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? GetUserId()
        {
            var result = string.Empty;
            if(_httpContextAccessor?.HttpContext != null)
                result = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
    }
}
