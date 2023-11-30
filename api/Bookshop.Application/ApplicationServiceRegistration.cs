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
            //services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(GetAllQueryHandler<Category>).Assembly));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            RegisterGenericGetAllQueryHandlers(services);
            RegisterGenericGetByIdQueryHandlers(services);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            services.AddTransient(typeof(IResponseFactory<>), typeof(ResponseFactory<>));
            return services;
        }

        private static void RegisterGenericGetAllQueryHandlers(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAllQuery<Category>, GetAllQueryResponse<Category>>, GetAllQueryHandler<Category>>();
            services.AddTransient<IRequestHandler<GetAllQuery<Address>, GetAllQueryResponse<Address>>, GetAllQueryHandler<Address>>();
            services.AddTransient<IRequestHandler<GetAllQuery<Book>, GetAllQueryResponse<Book>>, GetAllQueryHandler<Book>>();
            services.AddTransient<IRequestHandler<GetAllQuery<Author>, GetAllQueryResponse<Author>>, GetAllQueryHandler<Author>>();
            services.AddTransient<IRequestHandler<GetAllQuery<Customer>, GetAllQueryResponse<Customer>>, GetAllQueryHandler<Customer>>();
            services.AddTransient<IRequestHandler<GetAllQuery<LineItem>, GetAllQueryResponse<LineItem>>, GetAllQueryHandler<LineItem>>();
            services.AddTransient<IRequestHandler<GetAllQuery<Order>, GetAllQueryResponse<Order>>, GetAllQueryHandler<Order>>();
            services.AddTransient<IRequestHandler<GetAllQuery<ShoppingCart>, GetAllQueryResponse<ShoppingCart>>, GetAllQueryHandler<ShoppingCart>>();
        }
        private static void RegisterGenericGetByIdQueryHandlers(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetByIdQuery<Category>, GetByIdQueryResponse<Category>>, GetByIdQueryHandler<Category>>();
            services.AddTransient<IRequestHandler<GetByIdQuery<Address>, GetByIdQueryResponse<Address>>, GetByIdQueryHandler<Address>>();
            services.AddTransient<IRequestHandler<GetByIdQuery<Book>, GetByIdQueryResponse<Book>>, GetByIdQueryHandler<Book>>();
            services.AddTransient<IRequestHandler<GetByIdQuery<Author>, GetByIdQueryResponse<Author>>, GetByIdQueryHandler<Author>>();
            services.AddTransient<IRequestHandler<GetByIdQuery<Customer>, GetByIdQueryResponse<Customer>>, GetByIdQueryHandler<Customer>>();
            services.AddTransient<IRequestHandler<GetByIdQuery<LineItem>, GetByIdQueryResponse<LineItem>>, GetByIdQueryHandler<LineItem>>();
            services.AddTransient<IRequestHandler<GetByIdQuery<Order>, GetByIdQueryResponse<Order>>, GetByIdQueryHandler<Order>>();
            services.AddTransient<IRequestHandler<GetByIdQuery<ShoppingCart>, GetByIdQueryResponse<ShoppingCart>>, GetByIdQueryHandler<ShoppingCart>>();
        }
    }
}
