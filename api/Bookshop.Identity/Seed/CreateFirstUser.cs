using Bookshop.Domain.Entities;
using Bookshop.Identity.Roles;
using Bookshop.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Identity.Seed
{
    public static class CreateFirstUser
    {
        public static async Task SeedAsync(UserManager<IdentityUserData> userManager, BookshopDbContext context)
        {
            var newAddress = new Address("streetOfUser1", "cityOfUser1", "CodeUser1", "countryOfUser1", "stateOfUser1");
            var newCustomer = new Customer("firstName","lastName", newAddress, newAddress);
            newCustomer.IdentityData.UserName = "admin";
            newCustomer.IdentityData.Email = "ali@gmail.com";
            var user = await userManager.FindByNameAsync(newCustomer?.IdentityData?.UserName);
            if (context.Customers != null && !context.Customers.Any()  && newCustomer != null)
            {
                await context.Customers.AddAsync(newCustomer);
                if (user == null)
                {
                    await userManager.CreateAsync(newCustomer.IdentityData, "admin");
                    await userManager.AddToRoleAsync(newCustomer.IdentityData, RoleNames.Administrator);
                }
                if (user != null && (await userManager.GetRolesAsync(user)).Count == 0)
                {
                    await userManager.AddToRoleAsync(user, RoleNames.Administrator);
                }
                context.SaveChanges();
            }
        }
    }
}
