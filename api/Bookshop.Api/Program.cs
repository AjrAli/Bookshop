using Bookshop.Domain.Entities;
using Bookshop.Identity.Seed;
using Bookshop.Persistence.Context;
using Bookshop.Persistence.Seed;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Use appsettings.{environment}.json
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Use the default appsettings.json
                .Build();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(config)
                .CreateBootstrapLogger();
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await CreateRoles.SeedAsync(roleManager);
                    var userManager = services.GetRequiredService<UserManager<IdentityUserData>>();
                    var dbContext = services.GetRequiredService<BookshopDbContext>();
                    await CreateFirstUser.SeedAsync(userManager, dbContext);
                    await DatabaseSeeder.SeedAsync(dbContext);
                    Log.Information("Starting web host");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while starting the application");
                }
            }

            await host.RunAsync();
        }


        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
