using Bookshop.Domain.Common;
using Bookshop.Domain.Extension;
using Microsoft.AspNetCore.Identity;

namespace Bookshop.Domain.Entities
{
    public class Customer : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private Customer() { }
        private Customer(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

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
        }
        // Relationships
        public long? ShippingAddressId { get; set; }
        private Address _shippingAddress;
        public Address ShippingAddress
        {
            get => LazyLoader.Load(this, ref _shippingAddress);
            set => _shippingAddress = value;
        }
        public long? BillingAddressId { get; set; }
        private Address _billingAddress;
        public Address BillingAddress
        {
            get => LazyLoader.Load(this, ref _billingAddress);
            set => _billingAddress = value;
        }

        public string? IdentityUserDataId { get; set; }
        public IdentityUserData IdentityData { get; set; } = new IdentityUserData();
        public long? ShoppingCartId { get; set; }
        private ShoppingCart _shoppingCart;
        public ShoppingCart ShoppingCart
        {
            get => LazyLoader.Load(this, ref _shoppingCart);
            set => _shoppingCart = value;
        }


        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
