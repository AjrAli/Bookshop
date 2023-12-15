using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Response.Contracts
{
    public interface ICommandResponse : IBaseResponse
    {
        bool IsSaveChangesAsyncCalled { get; set; }
        IList<string>? ValidationErrors { get; set; }
    }
}
