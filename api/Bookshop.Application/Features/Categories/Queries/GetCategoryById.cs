using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Responses;

namespace Bookshop.Application.Features.Categories.Queries
{
    public class GetCategoryById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
