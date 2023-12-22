using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.Customers
{
    public class GetCustomerById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
