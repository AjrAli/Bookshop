using Bookshop.Application.Contracts.Seed;
using Serilog;
using Serilog.Events;

namespace Bookshop.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Get the ASP.NET Core environment, default to "Development" if not specified
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            // Build configuration from appsettings files, allowing reload on change
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Use appsettings.{environment}.json
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Use the default appsettings.json
                .Build();
            // Configure Serilog logger with minimum level, console output, and read from configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(config)
                .CreateBootstrapLogger();
            // Build the host using CreateHostBuilder method
            var host = CreateHostBuilder(args).Build();

            // Use a scope to manage the lifetime of services
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Retrieve and seed necessary data using identity and application services
                    var seedIdentityService = services.GetRequiredService<ISeedIdentityService>();
                    var seedApplicationService = services.GetRequiredService<ISeedApplicationService>();
                    await seedApplicationService.SeedInfrastructureDataAsync();
                    await seedIdentityService.SeedIdentityDataAsync();
                    // Log successful application start
                    Log.Information("Starting web host");
                }
                catch (Exception ex)
                {
                    // Log fatal error if an exception occurs during startup
                    Log.Fatal(ex, "An error occurred while starting the application");
                }
            }
            // Run the host asynchronously
            await host.RunAsync();
        }

        // Configure the host builder with default settings, Serilog logger, and startup class
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
