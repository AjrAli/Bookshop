using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Responses;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Books.Queries
{
    public class GetBookByIdHandler : IQueryHandler<GetBookById, GetByIdResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBookByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse> Handle(GetBookById request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Books.AsQueryable();
            query = query.Include(x => x.Author)
                         .Include(x => x.Category)
                         .Include(x => x.Comments)
                            .ThenInclude(x => x.Customer)
                                .ThenInclude(x => x.IdentityData);
            var entity = await query.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(Book)} with Id : {request.Id} not found");
            }

            var sourceType = typeof(Book);
            var targetType = typeof(BookResponseDto);

            var dto = _mapper.Map(entity, sourceType, targetType);

            return new()
            {
                Dto = dto
            };
        }
    }
}
