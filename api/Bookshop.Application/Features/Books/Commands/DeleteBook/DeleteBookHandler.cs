using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Response;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Books.Commands.DeleteBook
{
    public class DeleteBookHandler : ICommandHandler<DeleteBook, CommandResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public DeleteBookHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> Handle(DeleteBook request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            _dbContext.Books.Remove(bookToDelete);
            DeleteImageOfBook(request, bookToDelete);
            return new()
            {
                Success = true,
                Message = $"Book {bookToDelete.Id} successfully deleted"
            };
        }
        private void DeleteImageOfBook(DeleteBook request, Book book)
        {
            // Delete image of the book
            var filePath = Path.Combine(request.UploadImageDirectory, book.ImageName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        public async Task ValidateRequest(DeleteBook request)
        {
            if (!await _dbContext.Books.AnyAsync(x => x.Id == request.Id))
                throw new BadRequestException($"Book: {request.Id} not found in the database.");
        }
    }
}
