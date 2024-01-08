namespace Bookshop.Persistence.Contracts
{
    public interface ILoggedInUserService
    {
        string? GetUserId();
        string? GetUserToken();
        string? GetAuthScheme();
    }   
}