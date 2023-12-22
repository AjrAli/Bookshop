using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.Categories
{
    public class GetCategoryById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
