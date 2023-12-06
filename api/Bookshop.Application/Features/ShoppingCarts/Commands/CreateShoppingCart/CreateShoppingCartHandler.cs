using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart
{
    public class CreateShoppingCartHandler : ICommandHandler<CreateShoppingCart, CreateShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateShoppingCartHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<CreateShoppingCartResponse> Handle(CreateShoppingCart request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            var newShoppingCart = await CreateNewShoppingCartFromDto(request.ShoppingCart);
            await StoreShoppingCartInDatabase(request, newShoppingCart, cancellationToken);
            var shoppingCartCreated = _mapper.Map<ShoppingCartDto>(_dbContext.ShoppingCarts.Include(x => x.LineItems).FirstOrDefault(x => x.CustomerId == request.ShoppingCart.CustomerId));
            return new()
            {
                ShoppingCart = shoppingCartCreated,
                Message = $"ShoppingCart successfully created"
            };
        }
        private async Task StoreShoppingCartInDatabase(CreateShoppingCart request, ShoppingCart shoppingCart, CancellationToken cancellationToken)
        {
            await _dbContext.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        private async Task<ShoppingCart> CreateNewShoppingCartFromDto(ShoppingCartDto shoppingCartDto)
        {
            var shoppingCart = new ShoppingCart {CustomerId = shoppingCartDto.CustomerId};
            foreach (var item in shoppingCartDto.Items)
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == item.BookId);
                shoppingCart.AddItem(book, item.Quantity);
            }
            return shoppingCart;
        }
        private async Task ValidateRequest(CreateShoppingCart request)
        {
            if (request.ShoppingCart == null)
                throw new ValidationException($"{nameof(request.ShoppingCart)}, ShoppingCart information is required");
            if (request.ShoppingCart.Items == null)
                throw new ValidationException($"No items are listed in the ShoppingCart");
            if (request.ShoppingCart.CustomerId == null)
                throw new ValidationException($"Customer undefined for the ShoppingCart");
            if (await _dbContext.ShoppingCarts.AnyAsync(x => x.CustomerId == request.ShoppingCart.CustomerId))
                throw new ValidationException($"Customer {request.ShoppingCart.CustomerId} has already a ShoppingCart, not possible to create mutliples ShoppingCart for same customer");
            foreach (var item in request.ShoppingCart.Items)
            {
                if (!await _dbContext.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new ValidationException($"BookId : {item.BookId}, not found in DB which invalid the ShoppingCart");
                if (item.Quantity <= 0)
                    throw new ValidationException($"Quantity : {item.Quantity} of the bookId {item.BookId}, not valid which invalid the ShoppingCart");
            }
        }
    }
}
