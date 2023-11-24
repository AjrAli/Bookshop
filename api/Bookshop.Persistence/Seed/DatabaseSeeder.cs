using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;

namespace Bookshop.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public async static Task SeedAsync(BookshopDbContext context)
        {
            SeedAddresses(context);
            await context.SaveChangesAsync();
        }

        private async static void SeedAddresses(BookshopDbContext context)
        {
            if (context.Addresses != null && !context.Addresses.Any())
            {
                var addresses = new List<Address>
                {
                    new Address("street1","city1","postalCode1","country1","state1"),
                    new Address("street2","city2","postalCode2","country2","state2")
                };

                await context.Addresses.AddRangeAsync(addresses);
            }
        }

    }
}
