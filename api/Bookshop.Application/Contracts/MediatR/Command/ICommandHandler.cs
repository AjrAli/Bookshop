using Bookshop.Application.Features.Response.Contracts;
using MediatR;

namespace Bookshop.Application.Contracts.MediatR.Command;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
                                                                                                where TResponse : ICommandResponse
{
    Task ValidateRequest(TCommand request);
}