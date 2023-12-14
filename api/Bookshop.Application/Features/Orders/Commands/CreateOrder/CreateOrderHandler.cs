using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Orders.Extension;
using Bookshop.Application.Features.Orders.Validation;
using Bookshop.Application.Features.Service;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : ICommandHandler<CreateOrder, CreateOrderResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;

        public CreateOrderHandler(BookshopDbContext dbContext, IMapper mapper, IStockService stockService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _stockService = stockService;
        }
        public async Task<CreateOrderResponse> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            var customer = await GetCustomerWithDetails(request.Order, cancellationToken);
            var shoppingCart = await GetShoppingCartWithDetails(customer, cancellationToken);
            var newOrder = await ProcessingOrderFromDto(request.Order, customer, shoppingCart, cancellationToken);
            shoppingCart.RemoveShoppingCartReferencesWithLineItems(_dbContext);
            await StoreOrderInDatabase(request, newOrder, cancellationToken);
            var isSaveChangesAsync = await SaveChangesAsync(cancellationToken);
            var orderCreated = await newOrder.ToMappedOrderDto(_dbContext, _mapper, cancellationToken);
            return new()
            {
                Order = orderCreated,
                Message = $"Order successfully created",
                IsSaveChangesAsyncCalled = isSaveChangesAsync
            };
        }
        public async Task ValidateRequest(CreateOrder request)
        {
            await request.Order.ValidateOrderRequest(_dbContext);
        }
        private async Task<Order> ProcessingOrderFromDto(OrderRequestDto orderDto,
                                                         Customer customer,
                                                         ShoppingCart shoppingCart,
                                                         CancellationToken cancellationToken)
        {
            var methodOfpaymentEnum = Enum.Parse<CreditCards>(orderDto.MethodOfPayment);
            try
            {
                var order = new Order(methodOfpaymentEnum, customer, shoppingCart.LineItems);
                _stockService.UpdateStockQuantities(shoppingCart.LineItems);
                return order;
            }
            catch (InsufficientBookQuantityException ex)
            {
                HandleInsufficientQuantityException(ex, shoppingCart, cancellationToken);
                shoppingCart.UpdateShoppingCartTotal(_dbContext);
                await _dbContext.SaveChangesAsync(cancellationToken);
                throw;
            }

        }

        private void HandleInsufficientQuantityException(InsufficientBookQuantityException ex, ShoppingCart shoppingCart, CancellationToken cancellationToken)
        {
            foreach (var book in ex.BooksKeyWithQtValue.Keys)
            {
                HandleBookForInsufficientQuantity(book, shoppingCart);
            }
        }

        private void HandleBookForInsufficientQuantity(Book book, ShoppingCart shoppingCart)
        {
            var item = shoppingCart?.LineItems?.FirstOrDefault(x => x.BookId == book.Id);
            shoppingCart?.UpdateCartItem(book, book.Quantity);
            RemoveLineItemIfNoStock(book, item);
        }

        private void RemoveLineItemIfNoStock(Book book, LineItem? item)
        {
            if (book.Quantity == 0 && item != null)
                _dbContext.LineItems.Remove(item);
        }

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

        private async Task<Customer?> GetCustomerWithDetails(OrderRequestDto orderDto, CancellationToken cancellationToken)
        {
            return await _dbContext.Customers
                                           .Include(x => x.ShippingAddress)
                                                .ThenInclude(x => x.LocationPricing)
                                           .FirstOrDefaultAsync(x => x.IdentityUserDataId == orderDto.UserId, cancellationToken);
        }

        private async Task StoreOrderInDatabase(CreateOrder request, Order order, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == order.Customer.Id, cancellationToken);
            foreach (var item in order.LineItems)
            {
                item.Order = order;
            }
            customer.Orders = new List<Order> { order };
            await _dbContext.Orders.AddAsync(order, cancellationToken);
            _dbContext.Customers.Update(customer);
        }
        private async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
