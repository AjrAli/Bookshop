using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bookshop.Persistence.Context;

namespace Bookshop.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookshopDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BookshopDbConnectionString")));
            return services;
        }
    }
}
