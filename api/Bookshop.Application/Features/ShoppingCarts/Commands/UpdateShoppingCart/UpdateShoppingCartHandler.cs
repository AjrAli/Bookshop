using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Application.Features.ShoppingCarts.Validation;
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
            GroupItemsByBookId(request.ShoppingCart);
            var updatedShoppingCart = await UpdateShoppingCartFromDto(request.ShoppingCart);
            UpdateShoppingCartInDatabase(request.ShoppingCart, updatedShoppingCart);
            RemoveShoppingCartInDatabaseIfNoItems(updatedShoppingCart);
            await SaveChangesAsync(request, cancellationToken);
            var shoppingCartUpdated = await updatedShoppingCart.ToMappedShoppingCartDto(_dbContext, _mapper, cancellationToken);
            return new()
            {
                ShoppingCart = shoppingCartUpdated,
                Message = $"ShoppingCart successfully updated"
            };
        }
        private async Task SaveChangesAsync(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        private void RemoveShoppingCartInDatabaseIfNoItems(ShoppingCart shoppingCart)
        {
            if (shoppingCart.LineItems.Count == 0)
            {
                shoppingCart.RemoveShoppingCartFromCustomer(_dbContext);
                _dbContext.ShoppingCarts.Remove(shoppingCart);
            }
        }
        private void UpdateShoppingCartInDatabase(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            var itemsToRemove = LineItemsToRemove(shoppingCartDto, shoppingCart);
            foreach (var item in itemsToRemove)
                shoppingCart.UpdateCartItem(item.Book, 0);
            if (shoppingCart.LineItems.Count > 0)
            {
                _dbContext.ShoppingCarts.Update(shoppingCart);
                _dbContext.LineItems.RemoveRange(itemsToRemove);
            }
        }
        private ICollection<LineItem>? LineItemsToRemove(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            return shoppingCart.LineItems.Where(x => (!shoppingCartDto.Items.Any(y => y.BookId == x.BookId)) && x.BookId != 0)?.ToList();
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
                if (book.Quantity > 0)
                    shoppingCartExisting.UpdateCartItem(book, item.Quantity);
                else
                    throw new ValidationException($"Book: {book.Title} is not anymore available");
            }
            return shoppingCartExisting;
        }
        public async Task ValidateRequest(UpdateShoppingCart request)
        {
            await request.ShoppingCart.ValidateUpdateShoppingCartRequest(_dbContext);
        }
    }
}
