using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetByIdHandler<T> : IQueryHandler<GetById<T>, GetByIdResponse<T>> where T : class
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse<T>> Handle(GetById<T> request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            query = (request.NavigationPropertyConfigurations != null) ?
                query.ApplyIncludesAndThenIncludes(request.NavigationPropertyConfigurations) : query;
            var entity = await query.FirstOrDefaultAsync(x => EF.Property<object>(x, "Id") == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(T)} with Id : {request.Id} not found");
            }

            var dto = _mapper.Map<T>(entity);

            return new()
            {
                Dto = dto
            };
        }
    }
}
