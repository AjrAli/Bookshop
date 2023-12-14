using MediatR;

namespace Bookshop.Application.Contracts.MediatR.Command;

public interface ICommand<out TResponse> : IRequest<TResponse> where TResponse : ICommandResponse
{
}