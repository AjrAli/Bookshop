namespace Bookshop.Application.Features.Response
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse() { Success = false; }
        public ErrorResponse(string message)
            : base(message, success: false)
        {
        }
    }
}
