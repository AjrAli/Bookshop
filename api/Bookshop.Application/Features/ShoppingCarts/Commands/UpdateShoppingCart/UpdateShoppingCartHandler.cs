using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Service;
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
        private readonly IStockService _stockService;

        public UpdateShoppingCartHandler(BookshopDbContext dbContext, IMapper mapper, IStockService stockService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _stockService = stockService;
        }
        public async Task<UpdateShoppingCartResponse> Handle(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            GroupItemsByBookId(request.ShoppingCart);
            var updatedShoppingCart = await UpdateShoppingCartFromDto(request.ShoppingCart, cancellationToken);
            UpdateShoppingCartInDatabase(request.ShoppingCart, updatedShoppingCart);
            await SaveChangesAsync(request, cancellationToken);
            var shoppingCartUpdated = await updatedShoppingCart.ToMappedShoppingCartDto(_dbContext, _mapper, cancellationToken);
            return new()
            {
                ShoppingCart = shoppingCartUpdated,
                Message = $"ShoppingCart successfully updated",
                Details = updatedShoppingCart.GetQuantityMismatchMessage(request.ShoppingCart.Items)
            };
        }
        private async Task SaveChangesAsync(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        private void UpdateShoppingCartInDatabase(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            var itemsToRemove = LineItemsToRemove(shoppingCartDto, shoppingCart);
            foreach (var item in itemsToRemove)
                shoppingCart.UpdateCartItem(item.Book, 0);
            if (shoppingCart.LineItems.Count > 0)
            {
                shoppingCart.UpdateShoppingCartTotal(_dbContext);
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
        private async Task<ShoppingCart> UpdateShoppingCartFromDto(ShoppingCartRequestDto shoppingCartDto, CancellationToken cancellationToken)
        {
            ShoppingCart? shoppingCartExisting = await GetExistingShoppingCartOfUser(shoppingCartDto, cancellationToken);
            try
            {
                var booksIdRequest = shoppingCartDto.Items.Select(x => x.BookId).ToList();
                var booksRequestedFromDB = _dbContext.Books.Where(x => booksIdRequest.Any(y => y == x.Id)).ToList();
                if (_stockService.CheckBookStockQuantities(booksRequestedFromDB))
                {
                    await UpdateShoppingCartItems(shoppingCartDto, shoppingCartExisting, cancellationToken);
                }
            }
            catch (InsufficientBookQuantityException ex)
            {
                if (ex.BooksInvalid.Count > 0 && shoppingCartExisting.LineItems.Count > 0)
                {
                    foreach (var book in ex.BooksInvalid)
                    {
                        RemoveLineItemOfOutOfStockBook(shoppingCartExisting, book);
                    }
                    shoppingCartExisting.UpdateShoppingCartTotal(_dbContext);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
                throw;
            }
            return shoppingCartExisting;
        }

        private void RemoveLineItemOfOutOfStockBook(ShoppingCart? shoppingCartExisting, Book book)
        {
            var itemToDelete = shoppingCartExisting.LineItems.FirstOrDefault(x => x.BookId == book.Id);
            if (itemToDelete != null)
            {
                _dbContext.LineItems.Remove(itemToDelete);
                shoppingCartExisting.LineItems.Remove(itemToDelete);
            }
        }

        private async Task<ShoppingCart?> GetExistingShoppingCartOfUser(ShoppingCartRequestDto shoppingCartDto, CancellationToken cancellationToken)
        {
            return await _dbContext.ShoppingCarts
                                   .Include(x => x.Customer)
                                   .Include(x => x.LineItems)
                                        .ThenInclude(x => x.Book)
                                   .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == shoppingCartDto.UserId, cancellationToken);
        }

        private async Task UpdateShoppingCartItems(ShoppingCartRequestDto shoppingCartDto, ShoppingCart? shoppingCartExisting, CancellationToken cancellationToken)
        {
            foreach (var item in shoppingCartDto.Items)
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == item.BookId, cancellationToken) ?? throw new ValidationException($"BookId: {item.BookId} not found in the database.");
                shoppingCartExisting.UpdateCartItem(book, item.Quantity);
            }
        }

        public async Task ValidateRequest(UpdateShoppingCart request)
        {
            await request.ShoppingCart.ValidateUpdateShoppingCartRequest(_dbContext);
        }
    }
}
