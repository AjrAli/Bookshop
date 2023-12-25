using Bookshop.Application.Configuration;
using Bookshop.Application.Contracts.Seed;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Identity.Service
{
    public class SeedIdentityService : ISeedIdentityService
    {
        private readonly UserManager<IdentityUserData> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BookshopDbContext _dbContext;

        public SeedIdentityService(UserManager<IdentityUserData> userManager, RoleManager<IdentityRole> roleManager, BookshopDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task SeedIdentityDataAsync()
        {
            await SeedRoles();
            await SeedUser();
        }


        private async Task SeedRoles()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                var listRoles = new List<IdentityRole>()
                {
                    new IdentityRole(RoleNames.Administrator),
                    new IdentityRole(RoleNames.User),
                    new IdentityRole(RoleNames.Customer)
                };
                foreach (var role in listRoles)
                {
                    if (!await _roleManager.RoleExistsAsync(role.Name))
                    {
                        await _roleManager.CreateAsync(role);
                    }
                }
            }
        }

        private async Task SeedUser()
        {
            if (!await _userManager.Users.AnyAsync() && !await _dbContext.Customers.AnyAsync() && await _dbContext.LocationPricings.AnyAsync())
            {
                var locationPricing = await _dbContext.LocationPricings.FirstOrDefaultAsync();
                var shippingAddress = new Address("Rue Street", "City", "1000", locationPricing.Country, "Bruxelles")
                {
                    LocationPricing = locationPricing
                };
                var billingAddress = new Address("Rue Street", "City", "1000", locationPricing.Country, "Bruxelles");
                var admin = new Customer("firstUser", "firstLastName", shippingAddress, billingAddress);
                admin.IdentityData = new IdentityUserData("admin", "admin@gmail.com");
                // Add both Address entities to the context
                await _dbContext.Addresses.AddRangeAsync(shippingAddress, billingAddress);

                // Add the Customer entity to the context
                await _dbContext.Customers.AddAsync(admin);
                var result = await _userManager.CreateAsync(admin.IdentityData, "admin");
                if (result.Succeeded)
                {
                    var newUser = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(newUser, RoleNames.Administrator);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Errors: {result.Errors}");
                }
            }
        }
    }
}
