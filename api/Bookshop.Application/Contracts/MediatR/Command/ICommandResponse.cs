using MediatR;

namespace Bookshop.Application.Contracts.MediatR.Command
{
    public interface ICommandResponse
    {
        bool IsSaveChangesAsyncCalled { get; set; }
    }
}
