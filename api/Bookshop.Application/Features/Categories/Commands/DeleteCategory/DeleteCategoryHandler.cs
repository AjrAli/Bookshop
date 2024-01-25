using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Response;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : ICommandHandler<DeleteCategory, CommandResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public DeleteCategoryHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var categoryToDelete = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            _dbContext.Categories.Remove(categoryToDelete);
            return new()
            {
                Success = true,
                Message = $"Category {categoryToDelete.Id} successfully deleted"
            };
        }

        public async Task ValidateRequest(DeleteCategory request)
        {
            if (!await _dbContext.Categories.AnyAsync(x => x.Id == request.Id))
                throw new BadRequestException($"Category: {request.Id} not found in the database.");
        }
    }
}
