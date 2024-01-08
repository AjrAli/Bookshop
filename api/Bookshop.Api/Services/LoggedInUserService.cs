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
        public string? GetAuthScheme()
        {
            string? result = null;
            if (IsHttpContextNotNull())
                result = _httpContextAccessor.HttpContext.User?.Identity?.AuthenticationType; ;
            return result;
        }
        public string? GetUserId()
        {
            string? result = null;
            if (IsHttpContextNotNull())
                result = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
        public string? GetUserToken()
        {
            string? result = null;
            if (IsHeaderAuthorizationNotNullOrNotEmpty())
                result = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return result;
        }
        private bool IsHttpContextNotNull()
        {
            return _httpContextAccessor?.HttpContext != null;
        }
        private bool IsHeaderAuthorizationNotNullOrNotEmpty()
        {
            return IsHttpContextNotNull() &&
                   !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());
        }
    }
}
