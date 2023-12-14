using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Response
{
    public interface IBaseResponse : ICommandResponse
    {
        bool Success { get; set; }
        string? Message { get; set; }
        string? Details { get; set; }
        IList<string>? ValidationErrors { get; set; }
    }
}
