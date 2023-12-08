using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartHandler : ICommandHandler<UpdateShoppingCart, UpdateShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateShoppingCartHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<UpdateShoppingCartResponse> Handle(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            var shoppingCartDto = request.ShoppingCart;
            GroupItemsByBookId(shoppingCartDto);
            var updatedShoppingCart = await UpdateShoppingCartFromDto(shoppingCartDto);
            UpdateShoppingCartInDatabase(shoppingCartDto, updatedShoppingCart);
            await SaveChangesAsyncCalled(request, cancellationToken);
            var shoppingCartUpdated = await GetMappedShoppingCart(shoppingCartDto.CustomerId);
            return new()
            {
                ShoppingCart = shoppingCartUpdated,
                Message = $"ShoppingCart successfully updated"
            };
        }

        private async Task<ShoppingCartResponseDto?> GetMappedShoppingCart(long? customerId)
        {
            return await _dbContext.ShoppingCarts
                .Include(x => x.LineItems)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .Include(x => x.LineItems)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Category)
                .Where(x => x.CustomerId == customerId)
                .Select(x => _mapper.Map<ShoppingCartResponseDto>(x))
                .FirstOrDefaultAsync();
        }
        private async Task SaveChangesAsyncCalled(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        private void UpdateShoppingCartInDatabase(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            var itemsToRemove = LineItemsToRemove(shoppingCartDto, shoppingCart);
            foreach (var item in itemsToRemove)          
                shoppingCart.UpdateLineItem(item.Book, 0);         
            _dbContext.ShoppingCarts.Update(shoppingCart);
            _dbContext.LineItems.RemoveRange(itemsToRemove);
        }
        private ICollection<LineItem> LineItemsToRemove(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            return shoppingCart.LineItems.Where(x => !shoppingCartDto.Items.Any(y => y.BookId == x.BookId)).ToList();
        }
        private void GroupItemsByBookId(ShoppingCartRequestDto shoppingCartDto)
        {
            // Group if same book in multiple items of ShoppingCartDto
            shoppingCartDto.Items = shoppingCartDto.Items?.GroupBy(x => new { x.BookId, x.Id })
                                                         .Select(item => new ShopItemRequestDto
                                                         {
                                                             BookId = item.Key.BookId,
                                                             Id = item.Key.Id,
                                                             Quantity = item.Sum(x => x.Quantity)
                                                         }).Distinct().ToList();
        }
        private async Task<ShoppingCart> UpdateShoppingCartFromDto(ShoppingCartRequestDto shoppingCartDto)
        {
            var shoppingCartExisting = await _dbContext.ShoppingCarts.Include(x => x.LineItems).ThenInclude(x => x.Book).FirstOrDefaultAsync(x => x.Id == shoppingCartDto.Id);
            foreach (var item in shoppingCartDto.Items)
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == item.BookId) ?? throw new ValidationException($"BookId: {item.BookId} not found in the database.");
                shoppingCartExisting.UpdateLineItem(book, item.Quantity);
            }
            return shoppingCartExisting;
        }
        private async Task ValidateRequest(UpdateShoppingCart request)
        {
            if (request.ShoppingCart == null)
                throw new ValidationException($"{nameof(request.ShoppingCart)} is required.");

            var shoppingCart = request.ShoppingCart;
            if (shoppingCart.Id == 0)
                throw new ValidationException($"ShoppingCart {shoppingCart.Id} doesn't exist");
            if (shoppingCart.Items == null || !shoppingCart.Items.Any())
                throw new ValidationException("No items are listed in the ShoppingCart.");

            if (shoppingCart.CustomerId == null || shoppingCart.CustomerId == 0)
                throw new ValidationException("Customer undefined for the ShoppingCart.");

            if (!(await _dbContext.ShoppingCarts.AnyAsync(x => x.CustomerId == shoppingCart.CustomerId) &&
                await _dbContext.Customers.AnyAsync(x => x.ShoppingCartId == shoppingCart.Id)))
                throw new ValidationException($"ShoppingCart {shoppingCart.Id}with customer {shoppingCart.CustomerId} not found in Database");

            foreach (var item in shoppingCart.Items)
            {
                if (!await _dbContext.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new ValidationException($"BookId: {item.BookId} not found in the database.");

                if (item.Quantity < 0 || (item.Quantity == 0 && item.Id == 0))
                    throw new ValidationException($"Invalid quantity: {item.Quantity} for BookId: {item.BookId}.");
            }
        }
    }
}
