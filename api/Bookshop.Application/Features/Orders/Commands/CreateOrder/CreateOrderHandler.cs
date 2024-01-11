using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Orders.Validation;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Domain.Entities;
using Bookshop.Domain.Exceptions;
using Bookshop.Domain.Service;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Commands.CreateOrder
{
    /// <summary>
    /// Handles the command to create a new order.
    /// </summary>
    public class CreateOrderHandler : ICommandHandler<CreateOrder, CreateOrderResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;

        /// <summary>
        /// Initializes a new instance of the CreateOrderHandler class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="stockService">The stock service.</param>
        public CreateOrderHandler(BookshopDbContext dbContext, IMapper mapper, IStockService stockService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _stockService = stockService;
        }

        /// <summary>
        /// Handles the command to create a new order.
        /// </summary>
        /// <param name="request">The create order request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response containing the created order information.</returns>
        public async Task<CreateOrderResponse> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            // Validate the create order request
            await ValidateRequest(request);

            // Retrieve customer and shopping cart details
            var customer = await GetCustomerWithDetails(request.Order, cancellationToken);
            var shoppingCart = await GetShoppingCartWithDetails(customer, cancellationToken);

            // Process the order from DTO, handling stock and exceptions
            var newOrder = await ProcessingOrderFromDto(request.Order, customer, shoppingCart, cancellationToken);

            // Remove shopping cart references with line items
            shoppingCart.RemoveShoppingCartReferencesWithLineItems(_dbContext);

            // Store the order in the database
            await StoreOrderInDatabase(request, newOrder, cancellationToken);

            // Save changes to the database
            var isSaveChangesAsync = await SaveChangesAsync(cancellationToken);

            // Retrieve the last saved and mapped order details
            var orderCreated = _mapper.Map<OrderResponseDto>(newOrder);

            // Return the response
            return new CreateOrderResponse
            {
                Order = orderCreated,
                Message = "Order successfully created",
                IsSaveChangesAsyncCalled = isSaveChangesAsync
            };
        }

        /// <summary>
        /// Validates the create order request.
        /// </summary>
        /// <param name="request">The create order request.</param>
        public async Task ValidateRequest(CreateOrder request)
        {
            await request.Order.ValidateOrderRequest(_dbContext);
        }

        /// <summary>
        /// Processes the order from the DTO, handling stock and exceptions.
        /// </summary>
        /// <param name="orderDto">The order request DTO.</param>
        /// <param name="customer">The customer associated with the order.</param>
        /// <param name="shoppingCart">The shopping cart associated with the order.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the processed order.</returns>
        private async Task<Order> ProcessingOrderFromDto(
            OrderRequestDto orderDto,
            Customer customer,
            ShoppingCart shoppingCart,
            CancellationToken cancellationToken)
        {
            // Parse the method of payment from the order DTO
            var methodOfpaymentEnum = Enum.Parse<CreditCards>(orderDto.MethodOfPayment);

            try
            {
                // Create a new order and update stock quantities
                var order = new Order(methodOfpaymentEnum, customer, shoppingCart.LineItems);
                _stockService.UpdateStockQuantities(shoppingCart.LineItems);
                return order;
            }
            catch (InsufficientBookQuantityException ex)
            {
                // Handle insufficient book quantity exception
                HandleInsufficientQuantityException(ex, shoppingCart, cancellationToken);

                // Update shopping cart total and save changes
                shoppingCart.UpdateShoppingCartTotal(_dbContext);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Rethrow the exception
                throw;
            }
        }

        /// <summary>
        /// Handles the insufficient book quantity exception.
        /// </summary>
        /// <param name="ex">The insufficient book quantity exception.</param>
        /// <param name="shoppingCart">The shopping cart.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private void HandleInsufficientQuantityException(
            InsufficientBookQuantityException ex,
            ShoppingCart shoppingCart,
            CancellationToken cancellationToken)
        {
            // Loop through books with insufficient quantity and update shopping cart
            foreach (var book in ex.BooksKeyWithQtValue.Keys)
            {
                HandleBookForInsufficientQuantity(book, shoppingCart);
            }
        }

        /// <summary>
        /// Handles a book with insufficient quantity in the shopping cart.
        /// </summary>
        /// <param name="book">The book with insufficient quantity.</param>
        /// <param name="shoppingCart">The shopping cart.</param>
        private void HandleBookForInsufficientQuantity(Book book, ShoppingCart shoppingCart)
        {
            // Find the corresponding line item in the shopping cart
            var item = shoppingCart?.LineItems?.FirstOrDefault(x => x.BookId == book.Id);

            // Update the shopping cart item
            shoppingCart?.UpdateCartItem(book, book.Quantity);

            // Remove the line item if the book is out of stock
            RemoveLineItemIfNoStock(book, item);
        }

        /// <summary>
        /// Removes the line item from the shopping cart if there is no stock for the book.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="item">The line item in the shopping cart.</param>
        private void RemoveLineItemIfNoStock(Book book, LineItem? item)
        {
            if (book.Quantity == 0 && item != null)
                _dbContext.LineItems.Remove(item);
        }

        /// <summary>
        /// Retrieves the shopping cart with details for a customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the shopping cart with details.</returns>
        private async Task<ShoppingCart?> GetShoppingCartWithDetails(Customer? customer, CancellationToken cancellationToken)
        {
            return await _dbContext.ShoppingCarts
                .Include(x => x.LineItems)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .Include(x => x.LineItems)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.CustomerId == customer.Id, cancellationToken);
        }

        /// <summary>
        /// Retrieves the customer with details for an order request DTO.
        /// </summary>
        /// <param name="orderDto">The order request DTO.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the customer with details.</returns>
        private async Task<Customer?> GetCustomerWithDetails(OrderRequestDto orderDto, CancellationToken cancellationToken)
        {
            return await _dbContext.Customers
                .Include(x => x.ShippingAddress)
                .ThenInclude(x => x.LocationPricing)
                .FirstOrDefaultAsync(x => x.IdentityUserDataId == orderDto.UserId, cancellationToken);
        }

        /// <summary>
        /// Stores the order in the database.
        /// </summary>
        /// <param name="request">The create order request.</param>
        /// <param name="order">The order to store.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task StoreOrderInDatabase(CreateOrder request, Order order, CancellationToken cancellationToken)
        {
            // Retrieve the customer from the database
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == order.Customer.Id, cancellationToken);

            // Set the order for each line item
            foreach (var item in order.LineItems)
            {
                item.Order = order;
            }

            // Update the customer with the new order
            customer.Orders = new List<Order> { order };

            // Add the order to the database
            await _dbContext.Orders.AddAsync(order, cancellationToken);
            _dbContext.Customers.Update(customer);
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns true if changes are saved successfully.</returns>
        private async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
