using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetByIdHandler<Dto, T> : IQueryHandler<GetById<Dto>, GetByIdResponse>
        where Dto : class
        where T : class
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse> Handle(GetById<Dto> request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            var entity = await query.FirstOrDefaultAsync(x => EF.Property<object>(x, "Id") == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(T)} with Id : {request.Id} not found");
            }

            var sourceType = typeof(T);
            var targetType = typeof(Dto);

            var dto = _mapper.Map(entity, sourceType, targetType);

            return new()
            {
                Dto = dto
            };
        }
    }
}
