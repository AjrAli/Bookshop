using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Book;

namespace Bookshop.Application.Features.Books.Commands.UpdateBook
{
    public class UpdateBookHandler : ICommandHandler<UpdateBook, BookCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateBookHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BookCommandResponse> Handle(UpdateBook request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var authorRetrieved = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Id == request.Book.AuthorId);
            var categoryRetrieved = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Book.CategoryId);
            var bookExistingRetrieved = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == request.Id);
            DeletePreviousImageOfBook(request, bookExistingRetrieved);
            var editedBook = EditBookFromDto(request.Book, bookExistingRetrieved, authorRetrieved, categoryRetrieved);
            await CreateImageOfBook(request, editedBook);
            await EditBookInDatabase(editedBook, cancellationToken);
            var editedBookDto = _mapper.Map<BookResponseDto>(editedBook);
            return new BookCommandResponse()
            {
                Book = editedBookDto,
                Message = $"Book successfully updated",
                IsSaveChangesAsyncCalled = true
            };
        }
        private void DeletePreviousImageOfBook(UpdateBook request, Book bookExisting)
        {
            // Delete image of the book
            var filePath = Path.Combine(request.Book.UploadImageDirectory, bookExisting.ImageName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        private async Task CreateImageOfBook(UpdateBook request, Book book)
        {
            // Transform the byte[] to a new image
            var fileName = $"{book.Isbn}.{request.Book.ImageExtension}";
            var filePath = Path.Combine(request.Book.UploadImageDirectory, fileName);

            using (var stream = new MemoryStream(request.Book.Image))
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        private async Task EditBookInDatabase(Book book, CancellationToken cancellationToken)
        {
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private Book EditBookFromDto(BookRequestDto bookDto, Book bookExisting, Author author, Category category)
        {
            bookExisting.Title = bookDto.Title;
            bookExisting.Description = bookDto.Description;
            bookExisting.Publisher = bookDto.Publisher;
            bookExisting.Isbn = bookDto.Isbn;
            bookExisting.Price = bookDto.Price;
            bookExisting.Quantity = bookDto.Quantity;
            bookExisting.PageCount = bookDto.PageCount;
            bookExisting.Dimensions = bookDto.Dimensions;
            bookExisting.ImageUrl = $"{bookDto.ImageUrl}.{bookDto.ImageExtension}";
            bookExisting.ImageName = $"{bookDto.Isbn}.{bookDto.ImageExtension}";
            bookExisting.Language = Enum.Parse<Languages>(bookDto.Language);
            bookExisting.PublishDate = DateTime.Parse(bookDto.PublishDate);
            bookExisting.AuthorId = author.Id;
            bookExisting.Author = author;
            bookExisting.CategoryId = category.Id;
            bookExisting.Category = category;
            return bookExisting;
        }

        public async Task ValidateRequest(UpdateBook request)
        {
            if (!await _dbContext.Books.AnyAsync(x => x.Id == request.Id))
                throw new BadRequestException($"Book: {request.Id} not found in the database.");
            if (!await _dbContext.Authors.AnyAsync(x => x.Id == request.Book.AuthorId))
                throw new BadRequestException($"AuthorId: {request.Book.AuthorId} not found in the database.");
            if (!await _dbContext.Categories.AnyAsync(x => x.Id == request.Book.CategoryId))
                throw new BadRequestException($"CategoryId: {request.Book.CategoryId} not found in the database.");
        }
    }
}
