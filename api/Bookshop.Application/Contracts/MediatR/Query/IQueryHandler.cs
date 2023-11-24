﻿using MediatR;

namespace Bookshop.Application.Contracts.MediatR.Query;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    
}