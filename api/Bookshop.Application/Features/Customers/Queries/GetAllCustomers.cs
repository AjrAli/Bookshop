﻿using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Responses;

namespace Bookshop.Application.Features.Customers.Queries
{
    public class GetAllCustomers : IQuery<GetAllResponse>
    {
    }
}
