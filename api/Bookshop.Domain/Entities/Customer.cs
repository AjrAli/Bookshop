using Bookshop.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Bookshop.Domain.Entities
{
    public class Customer : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address ShippingAddress { get; set; } 
        public Address BillingAddress { get; set; }
        private Customer() { }

        // Constructor that initializes required fields
        public Customer(string firstName, 
                        string lastName, 
                        Address shippingAddress, 
                        Address billingAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
        }

        // Nested class to encapsulate identity-related data
        public class IdentityUserData : IdentityUser
        {
            public IdentityUserData(string userName, string email)
            {
                Email = email;
                UserName = userName;
                EmailConfirmed = true;
            }
            public IdentityUserData() 
            {
                EmailConfirmed = true;
            }

            // Relationships
            public long CustomerId { get; set; }
            public Customer? Customer { get; set; }
        }
        // Relationships
        public string IdentityUserDataId { get; set; }
        public IdentityUserData IdentityData { get; set; } = new IdentityUserData();
        public long? ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
