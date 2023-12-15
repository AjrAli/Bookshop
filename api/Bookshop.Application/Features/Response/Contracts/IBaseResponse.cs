namespace Bookshop.Application.Features.Response.Contracts
{
    public interface IBaseResponse
    {
        bool Success { get; set; }
        string? Message { get; set; }
        string? Details { get; set; }
    }
}
