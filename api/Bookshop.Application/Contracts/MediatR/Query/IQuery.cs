using MediatR;

namespace Bookshop.Application.Contracts.MediatR.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    
}