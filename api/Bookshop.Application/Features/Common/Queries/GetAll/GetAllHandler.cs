using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAllHandler<T> : IQueryHandler<GetAll<T>, GetAllResponse> where T : class
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllResponse> Handle(GetAll<T> request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.Set<T>().CountAsync(cancellationToken);

            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(T)} found");
            }

            var query = _dbContext.Set<T>().AsQueryable();
            query = (request.NavigationPropertyConfigurations != null) ?
                query.ApplyIncludesAndThenIncludes(request.NavigationPropertyConfigurations) : query;

            // Store the constant expressions in local variables
            var sourceType = typeof(T);
            var targetType = request.DtoType ?? typeof(T);

            var listDto = await query
                .Select(x => _mapper.Map(x, sourceType, targetType))
                .ToListAsync(cancellationToken: cancellationToken);

            return new GetAllResponse
            {
                ListDto = listDto
            };
        }
    }
}
