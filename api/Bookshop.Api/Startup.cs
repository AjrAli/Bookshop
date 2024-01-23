using Bookshop.Api.Middleware;
using Bookshop.Api.Services;
using Bookshop.Application;
using Bookshop.Application.Settings;
using Bookshop.Identity;
using Bookshop.Persistence;
using Bookshop.Persistence.Contracts;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

namespace Bookshop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Configures the services used by the application
        public void ConfigureServices(IServiceCollection services)
        {
            // Bind JwtSettings from configuration
            services.AddOptions<JwtSettings>().Bind(Configuration.GetSection("JwtSettings"));
            // Configure Swagger documentation
            AddSwagger(services);
            services.AddApplicationServices();
            services.AddPersistenceServices(Configuration);
            services.AddIdentityServices(Configuration);
            services.AddScoped<ILoggedInUserService, LoggedInUserService>();
            // Configure JSON options to ignore reference cycles
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddHttpContextAccessor();
            services.AddCors(options =>
            {
                options.AddPolicy("BookshopUI", builder =>
                {
                    var allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<string[]>();
                    builder.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });
        }
        // Configures Swagger generation and documentation
        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project : Bookshop API",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Configures the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            // Configure development-specific settings
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookshop API"); });
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"Client")),
                RequestPath = "/client"
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"Client", "img")),
                RequestPath = "/client/img"
            });
            // Configure Serilog request logging
            app.UseSerilogRequestLogging();
            // Configure routing, CORS, authentication, and authorization
            app.UseRouting();
            app.UseCors("BookshopUI");
            app.UseAuthentication();
            app.UseAuthorization();
            // Use custom exception handler middleware
            app.UseCustomExceptionHandler();
            // Configure endpoints for controllers and fallback to index.html
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
            // Register Serilog log closing on application stop
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}