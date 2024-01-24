using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Response;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorHandler : ICommandHandler<DeleteAuthor, CommandResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public DeleteAuthorHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> Handle(DeleteAuthor request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var authorToDelete = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            _dbContext.Authors.Remove(authorToDelete);
            return new()
            {
                Success = true,
                Message = $"Author {authorToDelete.Id} successfully deleted"
            };
        }

        public async Task ValidateRequest(DeleteAuthor request)
        {
            if (!await _dbContext.Authors.AnyAsync(x => x.Id == request.Id))
                throw new BadRequestException($"Author: {request.Id} not found in the database.");
        }
    }
}
