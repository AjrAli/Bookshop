using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAll<Dto> : IQuery<GetAllResponse> where Dto : class
    {
    }
}
