using Bookshop.Application.Features.Authors;
using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Categories;
using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Application.Features.Customers;
using Bookshop.Application.Features.Orders;
using Bookshop.Application.Features.PipelineBehaviours;
using Bookshop.Application.Features.Response;
using Bookshop.Application.Features.Response.Contracts;
using Bookshop.Application.Features.Service;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bookshop.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            RegisterGenericGetAllHandlers(services);
            RegisterGenericGetByIdHandlers(services);
            RegisterPipelineBehaviors(services);
            services.AddTransient<IStockService, StockService>();
            return services;
        }

        private static void RegisterPipelineBehaviors(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            services.AddTransient(typeof(IResponseFactory<>), typeof(ResponseFactory<>));
        }

        private static void RegisterGenericGetAllHandlers(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<CategoryResponseDto>, GetAllResponse>, GetAllHandler<CategoryResponseDto, Category>>();
            services.AddTransient<IRequestHandler<GetAll<BookResponseDto>, GetAllResponse>, GetAllHandler<BookResponseDto, Book>>();
            services.AddTransient<IRequestHandler<GetAll<AuthorResponseDto>, GetAllResponse>, GetAllHandler<AuthorResponseDto, Author>>();
            services.AddTransient<IRequestHandler<GetAll<CustomerResponseDto>, GetAllResponse>, GetAllHandler<CustomerResponseDto, Customer>>();
            services.AddTransient<IRequestHandler<GetAll<OrderResponseDto>, GetAllResponse>, GetAllHandler<OrderResponseDto, Order>>();
            services.AddTransient<IRequestHandler<GetAll<ShoppingCartResponseDto>, GetAllResponse>, GetAllHandler<ShoppingCartResponseDto, ShoppingCart>>();
        }
        private static void RegisterGenericGetByIdHandlers(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetById<CategoryResponseDto>, GetByIdResponse>, GetByIdHandler<CategoryResponseDto, Category>>();
            services.AddTransient<IRequestHandler<GetById<BookResponseDto>, GetByIdResponse>, GetByIdHandler<BookResponseDto, Book>>();
            services.AddTransient<IRequestHandler<GetById<AuthorResponseDto>, GetByIdResponse>, GetByIdHandler<AuthorResponseDto, Author>>();
            services.AddTransient<IRequestHandler<GetById<CustomerResponseDto>, GetByIdResponse>, GetByIdHandler<CustomerResponseDto, Customer>>();
            services.AddTransient<IRequestHandler<GetById<OrderResponseDto>, GetByIdResponse>, GetByIdHandler<OrderResponseDto, Order>>();
            services.AddTransient<IRequestHandler<GetById<ShoppingCartResponseDto>, GetByIdResponse>, GetByIdHandler<ShoppingCartResponseDto, ShoppingCart>>();
        }
    }
}
