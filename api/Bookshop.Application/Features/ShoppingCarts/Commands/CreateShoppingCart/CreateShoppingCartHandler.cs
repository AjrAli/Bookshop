using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Application.Features.ShoppingCarts.Validation;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart
{
    /// <summary>
    /// Handles the command to create a new shopping cart.
    /// </summary>
    public class CreateShoppingCartHandler : ICommandHandler<CreateShoppingCart, CreateShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the CreateShoppingCartHandler class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public CreateShoppingCartHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the command to create a new shopping cart.
        /// </summary>
        /// <param name="request">The create shopping cart request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response containing the created shopping cart information.</returns>
        public async Task<CreateShoppingCartResponse> Handle(CreateShoppingCart request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);

            // Create a new shopping cart from the DTO
            var newShoppingCart = await CreateNewShoppingCartFromDto(request.ShoppingCart);

            // Store the shopping cart in the database
            await StoreShoppingCartInDatabase(request, newShoppingCart, cancellationToken);

            // Save changes to the database
            var isSaveChangesAsync = await SaveChangesAsync(request, cancellationToken);

            // Retrieve the last saved and mapped shopping cart details
            var shoppingCartCreated = _mapper.Map<ShoppingCartResponseDto>(newShoppingCart);

            // Return the response
            return new CreateShoppingCartResponse
            {
                ShoppingCart = shoppingCartCreated,
                Message = $"ShoppingCart successfully created",
                Details = newShoppingCart.GetQuantityMismatchMessage(request.ShoppingCart.Items),
                IsSaveChangesAsyncCalled = isSaveChangesAsync
            };
        }

        /// <summary>
        /// Stores the shopping cart in the database.
        /// </summary>
        /// <param name="request">The create shopping cart request.</param>
        /// <param name="shoppingCart">The shopping cart to store.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task StoreShoppingCartInDatabase(CreateShoppingCart request, ShoppingCart shoppingCart, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == shoppingCart.Customer.Id);
            customer.ShoppingCart = shoppingCart;
            await _dbContext.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            _dbContext.Customers.Update(customer);
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="request">The create shopping cart request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns true if save changes operation is successful.</returns>
        private async Task<bool> SaveChangesAsync(CreateShoppingCart request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Creates a new shopping cart from the DTO.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
        /// <returns>Returns the newly created shopping cart.</returns>
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
                    ?? throw new BadRequestException($"BookId: {item.BookId} not found in the database.");

                if (book.Quantity > 0)
                    shoppingCart.UpdateCartItem(book, item.Quantity);
                else
                    throw new BadRequestException($"Book: {book.Title} is not anymore available");
            }

            return shoppingCart;
        }

        /// <summary>
        /// Validates the create shopping cart request.
        /// </summary>
        /// <param name="request">The create shopping cart request.</param>
        /// <returns>Returns a task representing the validation process.</returns>
        public async Task ValidateRequest(CreateShoppingCart request)
        {
            await request.ShoppingCart.ValidateCreateShoppingCartRequest(_dbContext);
        }
    }
}
