﻿using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Application.Features.ShoppingCarts.Validation;
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
            var isSaveChangesAsync = await SaveChangesAsync(request, cancellationToken);
            var shoppingCartCreated = await newShoppingCart.LastSavedToMappedShoppingCartDto(_dbContext, _mapper, cancellationToken);

            return new()
            {
                ShoppingCart = shoppingCartCreated,
                Message = $"ShoppingCart successfully created",
                Details = newShoppingCart.GetQuantityMismatchMessage(request.ShoppingCart.Items),
                IsSaveChangesAsyncCalled = isSaveChangesAsync
            };
        }
        private async Task StoreShoppingCartInDatabase(CreateShoppingCart request, ShoppingCart shoppingCart, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == shoppingCart.Customer.Id);
            customer.ShoppingCart = shoppingCart;
            await _dbContext.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            _dbContext.Customers.Update(customer);
        }
        private async Task<bool> SaveChangesAsync(CreateShoppingCart request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        private async Task<ShoppingCart> CreateNewShoppingCartFromDto(ShoppingCartRequestDto shoppingCartDto)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.IdentityUserDataId == shoppingCartDto.UserId);
            var shoppingCart = new ShoppingCart(customer);
            // Group if same book in multiple items of ShoppingCartDto
            shoppingCartDto.Items = shoppingCartDto.Items?.GroupBy(x => new { x.BookId, x.Id })
                                                         .Select(item => new ShopItemRequestDto
                                                         {
                                                             BookId = item.Key.BookId,
                                                             Id = item.Key.Id,
                                                             Quantity = item.Sum(x => x.Quantity)
                                                         }).Distinct().ToList();
            foreach (var item in shoppingCartDto.Items)
            {
                var book = await _dbContext.Books
                    .FirstOrDefaultAsync(x => x.Id == item.BookId)
                    ?? throw new ValidationException($"BookId: {item.BookId} not found in the database.");
                if (book.Quantity > 0)
                    shoppingCart.UpdateCartItem(book, item.Quantity);
                else
                    throw new ValidationException($"Book: {book.Title} is not anymore available");
            }

            return shoppingCart;
        }
        public async Task ValidateRequest(CreateShoppingCart request)
        {
            await request.ShoppingCart.ValidateCreateShoppingCartRequest(_dbContext);
        }
    }
}
