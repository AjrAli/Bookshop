using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Book;

namespace Bookshop.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookHandler : ICommandHandler<CreateBook, BookCommandResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateBookHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BookCommandResponse> Handle(CreateBook request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);
            var authorRetrieved = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Id == request.Book.AuthorId);
            var categoryRetrieved = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Book.CategoryId);
            var newBook = CreateNewBookFromDto(request.Book, authorRetrieved, categoryRetrieved);
            await StoreBookInDatabase(newBook, cancellationToken);
            var newBookCreated = _mapper.Map<BookResponseDto>(newBook);
            // Transform the byte[] to a new image
            var fileName = $"{newBookCreated.Isbn}.{request.Book.ImageExtension}";
            var filePath = Path.Combine(request.Book.UploadImageDirectory, fileName);

            using (var stream = new MemoryStream(request.Book.Image))
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
            return new BookCommandResponse()
            {
                Book = newBookCreated,
                Message = $"Book successfully created",
                IsSaveChangesAsyncCalled = true
            };
        }

        private async Task StoreBookInDatabase(Book book, CancellationToken cancellationToken)
        {
            await _dbContext.Books.AddAsync(book, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private Book CreateNewBookFromDto(BookRequestDto bookDto, Author author, Category category)
        {
            return new Book(bookDto.Title,
                bookDto.Description,
                bookDto.Publisher,
                bookDto.Isbn,
                bookDto.Price,
                bookDto.Quantity,
                bookDto.PageCount,
                bookDto.Dimensions,
                $"{bookDto.ImageUrl}.{bookDto.ImageExtension}",
                $"{bookDto.Isbn}.{bookDto.ImageExtension}",
                Enum.Parse<Languages>(bookDto.Language),
                DateTime.Parse(bookDto.PublishDate),
                author,
                category);
        }

        public async Task ValidateRequest(CreateBook request)
        {
            if (!await _dbContext.Authors.AnyAsync(x => x.Id == request.Book.AuthorId))
                throw new BadRequestException($"AuthorId: {request.Book.AuthorId} not found in the database.");
            if (!await _dbContext.Categories.AnyAsync(x => x.Id == request.Book.CategoryId))
                throw new BadRequestException($"CategoryId: {request.Book.CategoryId} not found in the database.");
        }
    }
}
