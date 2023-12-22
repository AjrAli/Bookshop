using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetById<Dto> : IQuery<GetByIdResponse> where Dto : class
    {
        public object? Id { get; set; }
    }
}
