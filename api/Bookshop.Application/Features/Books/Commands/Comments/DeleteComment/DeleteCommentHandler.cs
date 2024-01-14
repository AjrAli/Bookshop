using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Response;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Books.Commands.Comments.DeleteComment
{
    public class DeleteCommentHandler : ICommandHandler<DeleteComment, CommandResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public DeleteCommentHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> Handle(DeleteComment request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var commentToDelete = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (commentToDelete == null)
            {
                throw new NotFoundException($"{nameof(Comment)} {request.Id} is not found for current user");
            }

            _dbContext.Comments.Remove(commentToDelete);
            return new()
            {
                Success = true,
                Message = $"Comment {commentToDelete.Id} successfully deleted"
            };
        }

        public async Task ValidateRequest(DeleteComment request)
        {
            if (!await _dbContext.Comments.AnyAsync(x => x.Id == request.Id))
                throw new BadRequestException($"Comment: {request.Id} not found in the database.");
        }
    }
}
