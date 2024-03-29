﻿using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Responses;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Customers.Queries
{
    public class GetCustomerByIdHandler : IQueryHandler<GetCustomerById, GetByIdResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCustomerByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse> Handle(GetCustomerById request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Customers.AsQueryable();
            query = query.Include(x => x.BillingAddress)
                         .Include(x => x.ShippingAddress)
                         .Include(x => x.IdentityData);
            var entity = await query.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(Customer)} with Id : {request.Id} not found");
            }

            var sourceType = typeof(Customer);
            var targetType = typeof(CustomerResponseDto);

            var dto = _mapper.Map(entity, sourceType, targetType);

            return new()
            {
                Dto = dto
            };
        }
    }
}
