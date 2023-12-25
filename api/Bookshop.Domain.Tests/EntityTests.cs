using Bookshop.Domain.Entities;

namespace Bookshop.Domain.Tests
{
    [TestClass]
    public class EntityTests
    {
        [TestClass]
        public class ShoppingCartTests
        {
            [TestMethod]
            public void ShoppingCart_AddItem_ShouldAddNewItem()
            {
                // Arrange
                var shoppingCart = new ShoppingCart(null);
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8","URL", Book.Languages.English, DateTime.Now, author, category);

                // Act
                shoppingCart.UpdateCartItem(book, 2);

                // Assert
                Assert.AreEqual(1, shoppingCart.LineItems.Count);
                Assert.AreEqual(2, shoppingCart.LineItems.First().Quantity);
            }

            [TestMethod]
            public void ShoppingCart_AddItem_ShouldIncreaseQuantityForExistingItem()
            {
                // Arrange
                var shoppingCart = new ShoppingCart(null);
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);

                // Act
                shoppingCart.UpdateCartItem(book, 2);
                shoppingCart.UpdateCartItem(book, 3);

                // Assert
                Assert.AreEqual(1, shoppingCart.LineItems.Count);
                Assert.AreEqual(3, shoppingCart.LineItems.First().Quantity);
            }

            [TestMethod]
            public void ShoppingCart_ReduceItem_ShouldReduceQuantityForExistingItem()
            {
                // Arrange
                var shoppingCart = new ShoppingCart(null);
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);

                // Act
                shoppingCart.UpdateCartItem(book, 5);
                shoppingCart.UpdateCartItem(book, 3);

                // Assert
                Assert.AreEqual(1, shoppingCart.LineItems.Count);
                Assert.AreEqual(3, shoppingCart.LineItems.First().Quantity);
            }

            [TestMethod]
            public void ShoppingCart_ReduceItem_ShouldRemoveItemWhenQuantityReachesZero()
            {
                // Arrange
                var shoppingCart = new ShoppingCart(null);
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);

                // Act
                shoppingCart.UpdateCartItem(book, 2);
                shoppingCart.UpdateCartItem(book, 0);

                // Assert
                Assert.AreEqual(0, shoppingCart.LineItems.Count);
            }
            [TestMethod]
            public void ShoppingCart_Total_ShouldReturnCorrectTotalCalculated()
            {
                // Arrange
                var shoppingCart = new ShoppingCart(null);
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);

                // Act
                shoppingCart.UpdateCartItem(book, 2);
                shoppingCart.UpdateCartItem(book, 3);

                // Assert
                Assert.AreEqual(60, shoppingCart.Total);
            }
        }
        [TestClass]
        public class LineItemTests
        {
            [TestMethod]
            public void LineItem_CalculatePrice_ShouldReturnCorrectPrice()
            {
                // Arrange
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);
                var lineItem = new LineItem(book, 3);

                // Act
                var totalPrice = lineItem.Price;

                // Assert
                Assert.AreEqual(60.0m, totalPrice);
            }
            [TestMethod]
            public void LineItem_UpdatingQuantity_ShouldRecalculatePriceCorrectly()
            {
                // Arrange
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);
                var lineItem = new LineItem(book, 3);

                // Act
                var totalPriceBeforeUpdate = lineItem.Price;
                lineItem.Quantity += 3;
                var totalPriceAfterUpdate = lineItem.Price;

                // Assert
                Assert.AreEqual(60.0m, totalPriceBeforeUpdate);
                Assert.AreEqual(120.0m, totalPriceAfterUpdate);
            }
            [TestMethod]
            public void LineItem_UpdatingQuantityBeyondAvailableStock_ShouldRecalculatePriceWithMaxBookQuantity()
            {
                // Arrange
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 10.0m, 2, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);
                var lineItem = new LineItem(book, 1);

                // Act
                var totalPriceBeforeUpdate = lineItem.Price;
                // Attempt to increase quantity beyond available stock
                lineItem.Quantity += 3;
                var totalPriceAfterUpdate = lineItem.Price;

                // Assert
                Assert.AreEqual(10.0m, totalPriceBeforeUpdate);
                Assert.AreEqual(20.0m, totalPriceAfterUpdate);
            }
        }
        [TestClass]
        public class OrderTests
        {
            [TestMethod]
            public void Order_CalculateTotalOrder_ShouldReturnCorrectTotalAmount()
            {
                // Arrange
                var firstName = "John";
                var lastName = "Doe";
                var locationpricing = new LocationPricing("Belgium", 10m, 10m);
                var shippingAddress = new Address("streetOfUser1", "cityOfUser1", "CodeUser1", "Belgium", "stateOfUser1") 
                { 
                    LocationPricing = locationpricing
                };
                var billingAddress = new Address("billstreetOfUser1", "billcityOfUser1", "billCodeUser1", "billcountryOfUser1", "billstateOfUser1");
                var customer = new Customer(firstName, lastName, shippingAddress, billingAddress);            
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book1 = new Book("Test Book 1", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);
                var book2 = new Book("Test Book 2", "Description", "Publisher", "ISBN456", 30.0m, 50, 150, "6x9", "URL", Book.Languages.French, DateTime.Now, author, category);
                var lineItem1 = new LineItem(book1, 2);
                var lineItem2 = new LineItem(book2, 1);

                // Act
                var order = new Order(Order.CreditCards.Visa, customer, new List<LineItem> { lineItem1, lineItem2 });

                // Assert
                Assert.AreEqual(87m, order.Total); // Assuming correct calculation based on provided logic
            }
        }
        [TestClass]
        public class CustomerTests
        {
            [TestMethod]
            public void Customer_Constructor_ShouldInitializeProperties()
            {
                // Arrange
                var firstName = "John";
                var lastName = "Doe";
                var shippingAddress = new Address("streetOfUser1", "cityOfUser1", "CodeUser1", "countryOfUser1", "stateOfUser1");
                var billingAddress = new Address("billstreetOfUser1", "billcityOfUser1", "billCodeUser1", "billcountryOfUser1", "billstateOfUser1");

                // Act
                var customer = new Customer(firstName, lastName, shippingAddress, billingAddress);

                // Assert
                Assert.AreEqual(firstName, customer.FirstName);
                Assert.AreEqual(lastName, customer.LastName);
                Assert.AreEqual(shippingAddress, customer.ShippingAddress);
                Assert.AreEqual(billingAddress, customer.BillingAddress);
            }

            [TestMethod]
            public void Customer_Orders_ShouldBeInitializedWithRelationship()
            {
                // Arrange
                var locationpricing = new LocationPricing("Belgium", 10m, 10m);
                var shippingAddress = new Address("streetOfUser1", "cityOfUser1", "CodeUser1", "Belgium", "stateOfUser1")
                {
                    LocationPricing = locationpricing
                };
                var billingAddress = new Address("billstreetOfUser1", "billcityOfUser1", "billCodeUser1", "billcountryOfUser1", "billstateOfUser1");
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book1 = new Book("Test Book 1", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category);
                var book2 = new Book("Test Book 2", "Description", "Publisher", "ISBN456", 30.0m, 50, 150, "6x9", "URL", Book.Languages.French, DateTime.Now, author, category);
                var lineItem1 = new LineItem(book1, 2);
                var lineItem2 = new LineItem(book2, 1);
                var customer = new Customer("John", "Doe", shippingAddress, billingAddress);
                var order = new Order(Order.CreditCards.Visa, customer, new List<LineItem> { lineItem1, lineItem2 });

                // Act
                customer.Orders = new List<Order> { order};

                // Assert
                Assert.IsNotNull(customer.Orders);
                Assert.IsInstanceOfType(customer.Orders, typeof(List<Order>));
                Assert.IsTrue(customer.Orders.Count > 0);
            }

            [TestMethod]
            public void Customer_ShoppingCart_And_Order_ShouldWorkTogether()
            {
                // Arrange
                var locationpricing = new LocationPricing("Belgium", 10m, 10m);
                var shippingAddress = new Address("streetOfUser1", "cityOfUser1", "CodeUser1", "Belgium", "stateOfUser1")
                {
                    LocationPricing = locationpricing
                };
                var billingAddress = new Address("billstreetOfUser1", "billcityOfUser1", "billCodeUser1", "billcountryOfUser1", "billstateOfUser1");
                var customer = new Customer("John", "Doe", shippingAddress, billingAddress);
                var customer2 = new Customer("John2", "Doe2", shippingAddress, billingAddress);

                // Simulate adding books to the shopping cart
                var author = new Author("author1", "best author");
                var category = new Category("category1", "best category");
                var book1 = new Book("Test Book 1", "Description", "Publisher", "ISBN123", 20.0m, 50, 100, "5x8", "URL", Book.Languages.English, DateTime.Now, author, category) 
                { 
                    Id = 1
                };
                var book2 = new Book("Test Book 2", "Description", "Publisher", "ISBN456", 30.0m, 50, 150, "6x9", "URL", Book.Languages.French, DateTime.Now, author, category)
                {
                    Id = 2
                };
                customer.ShoppingCart = new ShoppingCart(customer);
                customer.ShoppingCart.UpdateCartItem(book1, 2);
                customer.ShoppingCart.UpdateCartItem(book2, 1);
                var listItems = new List<LineItem>
                {
                    new LineItem(book1, 2),
                    new LineItem(book2, 1),
                };
                customer2.ShoppingCart = new ShoppingCart(customer2, listItems);

                // Act
                // Simulate placing an order
                var order = new Order(Order.CreditCards.Visa, customer, new List<LineItem>(customer.ShoppingCart.LineItems));
                customer.Orders.Add(order);

                // Assert
                Assert.IsNotNull(customer.ShoppingCart);
                Assert.AreEqual(2, customer.ShoppingCart.LineItems.Count);
                Assert.AreEqual(3, customer.ShoppingCart.TotalItems());
                Assert.AreEqual(70, customer.ShoppingCart.Total);
                Assert.AreEqual(customer2.ShoppingCart.Total, customer.ShoppingCart.Total);
                Assert.AreEqual(87m, customer.Orders.FirstOrDefault()?.Total);
                Assert.AreEqual(1, customer.Orders.Count);
            }
        }

    }
}