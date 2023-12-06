using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Queries.GetAll;
using Bookshop.Application.Features.Common.Queries.GetById;
using Bookshop.Application.Features.PipelineBehaviours;
using Bookshop.Application.Features.Response;
using Bookshop.Domain.Common;
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
            services.AddTransient<IRequestHandler<GetAll<Category>, GetAllResponse<Category>>, GetAllHandler<Category>>();
            services.AddTransient<IRequestHandler<GetAll<Address>, GetAllResponse<Address>>, GetAllHandler<Address>>();
            services.AddTransient<IRequestHandler<GetAll<Book>, GetAllResponse<Book>>, GetAllHandler<Book>>();
            services.AddTransient<IRequestHandler<GetAll<Author>, GetAllResponse<Author>>, GetAllHandler<Author>>();
            services.AddTransient<IRequestHandler<GetAll<Customer>, GetAllResponse<Customer>>, GetAllHandler<Customer>>();
            services.AddTransient<IRequestHandler<GetAll<LineItem>, GetAllResponse<LineItem>>, GetAllHandler<LineItem>>();
            services.AddTransient<IRequestHandler<GetAll<Order>, GetAllResponse<Order>>, GetAllHandler<Order>>();
            services.AddTransient<IRequestHandler<GetAll<ShoppingCart>, GetAllResponse<ShoppingCart>>, GetAllHandler<ShoppingCart>>();
        }
        private static void RegisterGenericGetByIdHandlers(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetById<Category>, GetByIdResponse<Category>>, GetByIdHandler<Category>>();
            services.AddTransient<IRequestHandler<GetById<Address>, GetByIdResponse<Address>>, GetByIdHandler<Address>>();
            services.AddTransient<IRequestHandler<GetById<Book>, GetByIdResponse<Book>>, GetByIdHandler<Book>>();
            services.AddTransient<IRequestHandler<GetById<Author>, GetByIdResponse<Author>>, GetByIdHandler<Author>>();
            services.AddTransient<IRequestHandler<GetById<Customer>, GetByIdResponse<Customer>>, GetByIdHandler<Customer>>();
            services.AddTransient<IRequestHandler<GetById<LineItem>, GetByIdResponse<LineItem>>, GetByIdHandler<LineItem>>();
            services.AddTransient<IRequestHandler<GetById<Order>, GetByIdResponse<Order>>, GetByIdHandler<Order>>();
            services.AddTransient<IRequestHandler<GetById<ShoppingCart>, GetByIdResponse<ShoppingCart>>, GetByIdHandler<ShoppingCart>>();
        }
    }
}
