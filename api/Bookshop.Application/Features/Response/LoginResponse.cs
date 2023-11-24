namespace Bookshop.Application.Features.Response
{
    public class LoginResponse : BaseResponse
    {
        public LoginResponse() : base() 
        { 

        }
        public string Id { get; set; }
        public string? Token { get; set; }
    }
}
