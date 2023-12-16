﻿using Bookshop.Application.Contracts.MediatR.Query;
using System.Linq.Expressions;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAll<T> : IQuery<GetAllResponse> where T : class
    {
        public Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>>? NavigationPropertyConfigurations { get; set; }
        public Type? DtoType { get; set; }
    }
}
