using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Application.Features.PipelineBehaviours;
using Bookshop.Application.Features.Response;
using Bookshop.Application.Features.Response.Contracts;
using Bookshop.Application.Features.Service;
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
            services.AddTransient<IRequestHandler<GetAll<Category>, GetAllResponse>, GetAllHandler<Category>>();
            services.AddTransient<IRequestHandler<GetAll<Address>, GetAllResponse>, GetAllHandler<Address>>();
            services.AddTransient<IRequestHandler<GetAll<Book>, GetAllResponse>, GetAllHandler<Book>>();
            services.AddTransient<IRequestHandler<GetAll<Author>, GetAllResponse>, GetAllHandler<Author>>();
            services.AddTransient<IRequestHandler<GetAll<Customer>, GetAllResponse>, GetAllHandler<Customer>>();
            services.AddTransient<IRequestHandler<GetAll<LineItem>, GetAllResponse>, GetAllHandler<LineItem>>();
            services.AddTransient<IRequestHandler<GetAll<Order>, GetAllResponse>, GetAllHandler<Order>>();
            services.AddTransient<IRequestHandler<GetAll<ShoppingCart>, GetAllResponse>, GetAllHandler<ShoppingCart>>();
        }
        private static void RegisterGenericGetByIdHandlers(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetById<Category>, GetByIdResponse>, GetByIdHandler<Category>>();
            services.AddTransient<IRequestHandler<GetById<Address>, GetByIdResponse>, GetByIdHandler<Address>>();
            services.AddTransient<IRequestHandler<GetById<Book>, GetByIdResponse>, GetByIdHandler<Book>>();
            services.AddTransient<IRequestHandler<GetById<Author>, GetByIdResponse>, GetByIdHandler<Author>>();
            services.AddTransient<IRequestHandler<GetById<Customer>, GetByIdResponse>, GetByIdHandler<Customer>>();
            services.AddTransient<IRequestHandler<GetById<LineItem>, GetByIdResponse>, GetByIdHandler<LineItem>>();
            services.AddTransient<IRequestHandler<GetById<Order>, GetByIdResponse>, GetByIdHandler<Order>>();
            services.AddTransient<IRequestHandler<GetById<ShoppingCart>, GetByIdResponse>, GetByIdHandler<ShoppingCart>>();
        }
    }
}
