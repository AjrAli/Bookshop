using Bookshop.Identity.Roles;
using Microsoft.AspNetCore.Identity;

namespace Bookshop.Identity.Seed
{
    public static class CreateRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager != null && !roleManager.Roles.Any())
            {
                var listRoles = new List<IdentityRole>()
                {
                    new IdentityRole(RoleNames.Administrator),
                    new IdentityRole(RoleNames.User),
                    new IdentityRole(RoleNames.Customer)
                };
                foreach (var role in listRoles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
            }
        }
    }
}
