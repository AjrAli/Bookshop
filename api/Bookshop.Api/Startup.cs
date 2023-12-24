using Bookshop.Api.Middleware;
using Bookshop.Api.Services;
using Bookshop.Api.Utility;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<JwtSettings>().Bind(Configuration.GetSection("JwtSettings"));
            AddSwagger(services);
            services.AddApplicationServices();
            services.AddPersistenceServices(Configuration);
            services.AddIdentityServices(Configuration);
            services.AddScoped<ILoggedInUserService, LoggedInUserService>();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddHttpContextAccessor();
            services.AddCors(options =>
            {
                options.AddPolicy("BookshopUI", builder =>
                {
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

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

                c.OperationFilter<FileResultContentTypeOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
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
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors("BookshopUI");
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}