namespace Bookshop.Application.Features.Response
{
    public interface IBaseResponse
    {
        bool Success { get; set; }
        string? Message { get; set; }
        string? Details { get; set; }
        IList<string>? ValidationErrors { get; set; }
        long Count { get; set; }
    }
}
