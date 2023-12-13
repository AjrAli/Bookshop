using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Orders.Extension;
using Bookshop.Application.Features.Orders.Validation;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : ICommandHandler<CreateOrder, CreateOrderResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateOrderHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }
        public async Task<CreateOrderResponse> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            var customer = await GetCustomerWithDetails(request.Order, cancellationToken);
            var shoppingCart = await GetShoppingCartWithDetails(customer, cancellationToken);
            var newOrder = await ProcessingOrderFromDto(request.Order, customer, shoppingCart, cancellationToken);
            RemoveShoppingCartWithReferences(shoppingCart);
            await StoreOrderInDatabase(request, newOrder, cancellationToken);
            var orderCreated = await newOrder.ToMappedOrderDto(_dbContext, _mapper, cancellationToken);
            return new()
            {
                Order = orderCreated,
                Message = $"Order successfully created"
            };
        }
        public async Task ValidateRequest(CreateOrder request)
        {
            await request.Order.ValidateOrderRequest();
        }
        private void RemoveShoppingCartWithReferences(ShoppingCart shoppingCart)
        {
            shoppingCart.RemoveShoppingCartFromCustomer(_dbContext);
            foreach (var lineItem in shoppingCart.LineItems)
                lineItem.ShoppingCartId = null;
            _dbContext.ShoppingCarts.Remove(shoppingCart);
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
                order.UpdateStockQuantities();
                return order;
            }
            catch (InsufficientQuantityException ex)
            {
                await HandleInsufficientQuantityException(ex, shoppingCart, cancellationToken);
                throw;
            }

        }

        private async Task HandleInsufficientQuantityException(InsufficientQuantityException ex, ShoppingCart shoppingCart, CancellationToken cancellationToken)
        {
            foreach (var book in ex.BooksKeyWithQtValue.Keys)
            {
                shoppingCart.UpdateCartItem(book, book.Quantity);
                _dbContext.ShoppingCarts.Update(shoppingCart);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
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
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
    }
}
