using MediatR;

namespace Bookshop.Application.Contracts.MediatR.Command;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    
}