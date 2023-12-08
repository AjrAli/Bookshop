﻿using AutoMapper;
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
            var shoppingCartCreated = await GetMappedShoppingCart(request.ShoppingCart.CustomerId);
            return new()
            {
                ShoppingCart = shoppingCartCreated,
                Message = $"ShoppingCart successfully created"
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
        private async Task StoreShoppingCartInDatabase(CreateShoppingCart request, ShoppingCart shoppingCart, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == shoppingCart.CustomerId);
            customer.ShoppingCart = shoppingCart;
            await _dbContext.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        private async Task<ShoppingCart> CreateNewShoppingCartFromDto(ShoppingCartRequestDto shoppingCartDto)
        {
            var shoppingCart = new ShoppingCart { CustomerId = shoppingCartDto.CustomerId };
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

                shoppingCart.UpdateLineItem(book, item.Quantity);
            }

            return shoppingCart;
        }
        public async Task ValidateRequest(CreateShoppingCart request)
        {
            if (request.ShoppingCart == null)
                throw new ValidationException($"{nameof(request.ShoppingCart)} is required.");

            var shoppingCart = request.ShoppingCart;

            if (shoppingCart.Items == null || !shoppingCart.Items.Any())
                throw new ValidationException("No items are listed in the ShoppingCart.");

            if (shoppingCart.CustomerId == null || shoppingCart.CustomerId == 0)
                throw new ValidationException("Customer undefined for the ShoppingCart.");

            if (await _dbContext.ShoppingCarts.AnyAsync(x => x.CustomerId == shoppingCart.CustomerId) ||
                await _dbContext.Customers.AnyAsync(x => x.ShoppingCartId == shoppingCart.Id))
                throw new ValidationException($"Customer {shoppingCart.CustomerId} already has a ShoppingCart.");

            foreach (var item in shoppingCart.Items)
            {
                if (!await _dbContext.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new ValidationException($"BookId: {item.BookId} not found in the database.");

                if (item.Quantity <= 0)
                    throw new ValidationException($"Invalid quantity: {item.Quantity} for BookId: {item.BookId}.");
            }
        }
    }
}
