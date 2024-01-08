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
            string? result = null;
            if (_httpContextAccessor?.HttpContext != null)
                result = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
        public string? GetUserToken()
        {
            string? result = null;
            if (_httpContextAccessor?.HttpContext != null && !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()))
                result = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return result;
        }
        public string? GetAuthScheme()
        {
            string? result = null;
            if (_httpContextAccessor?.HttpContext != null)
                result = _httpContextAccessor.HttpContext.User?.Identity?.AuthenticationType; ;
            return result;
        }
    }
}
