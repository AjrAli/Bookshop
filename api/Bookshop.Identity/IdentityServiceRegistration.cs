using Bookshop.Application.Contracts.Seed;
using Bookshop.Application.Settings;
using Bookshop.Identity.Service;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Identity
{
    /// <summary>
    /// Handles the registration of Identity-related services.
    /// </summary>
    public static class IdentityServiceRegistration
    {
        /// <summary>
        /// Adds Identity-related services to the specified service collection.
        /// </summary>
        /// <param name="services">The service collection to which services are added.</param>
        /// <param name="configuration">The configuration settings.</param>
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve JWT settings from configuration
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            // Configure Identity with custom password policies
            services.AddIdentity<IdentityUserData, IdentityRole>(config =>
            {
                // Set minimum password length
                config.Password.RequiredLength = 4;
                // Require at least one non-alphanumeric character
                config.Password.RequireDigit = false;
                // Require at least one uppercase letter
                config.Password.RequireUppercase = false;
                // Do not require a non-alphanumeric character
                config.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<BookshopDbContext>().AddDefaultTokenProviders();

            // Configure authentication with JWT Bearer
            services.AddAuthentication(options =>
            {
                // Set the default authentication scheme for the application
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    // Configure JWT Bearer options
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validate the server's signature key
                        ValidateIssuerSigningKey = true,
                        // Validate the server's authority
                        ValidateIssuer = true, /*The "issuer" claim identifies the entity that issued (created and signed) the JWT. It indicates the authority or the party that generated the token.*/
                        // Validate the audience of the token
                        ValidateAudience = true, /*The "audience" claim identifies the recipients that the JWT is intended for. It specifies the intended audience or the target service/resource that should accept and process the JWT.*/
                        // Validate the token's expiration time
                        ValidateLifetime = true,
                        // Set the clock skew to zero to prevent token expiration errors
                        ClockSkew = TimeSpan.Zero,
                        // Set the valid issuer of the token
                        ValidIssuer = jwtSettings.Issuer,
                        // Set the valid audience of the token
                        ValidAudience = jwtSettings.Audience,
                        // Set the symmetric security key for validating the token signature
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.Key))
                    };
                });

            // Add SeedIdentityService as a transient service
            services.AddTransient<ISeedIdentityService, SeedIdentityService>();
        }
    }
}
