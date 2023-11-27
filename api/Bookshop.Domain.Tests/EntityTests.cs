using Bookshop.Domain.Entities;

namespace Bookshop.Domain.Tests
{
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void ShoppingCart_AddItem_ShouldAddNewItem()
        {
            // Arrange
            var shoppingCart = new ShoppingCart();
            var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 1, 100, "5x8", Book.Languages.English, DateTime.Now);

            // Act
            shoppingCart.AddItem(book, 2);

            // Assert
            Assert.AreEqual(1, shoppingCart.LineItems.Count);
            Assert.AreEqual(2, shoppingCart.LineItems.First().Quantity);
        }

        [TestMethod]
        public void ShoppingCart_AddItem_ShouldIncreaseQuantityForExistingItem()
        {
            // Arrange
            var shoppingCart = new ShoppingCart();
            var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 1, 100, "5x8", Book.Languages.English, DateTime.Now);

            // Act
            shoppingCart.AddItem(book, 2);
            shoppingCart.AddItem(book, 3);

            // Assert
            Assert.AreEqual(1, shoppingCart.LineItems.Count);
            Assert.AreEqual(5, shoppingCart.LineItems.First().Quantity);
        }

        [TestMethod]
        public void ShoppingCart_ReduceItem_ShouldReduceQuantityForExistingItem()
        {
            // Arrange
            var shoppingCart = new ShoppingCart();
            var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 1, 100, "5x8", Book.Languages.English, DateTime.Now);

            // Act
            shoppingCart.AddItem(book, 5);
            shoppingCart.RemoveItem(book, 3);

            // Assert
            Assert.AreEqual(1, shoppingCart.LineItems.Count);
            Assert.AreEqual(2, shoppingCart.LineItems.First().Quantity);
        }

        [TestMethod]
        public void ShoppingCart_ReduceItem_ShouldRemoveItemWhenQuantityReachesZero()
        {
            // Arrange
            var shoppingCart = new ShoppingCart();
            var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 1, 100, "5x8", Book.Languages.English, DateTime.Now);

            // Act
            shoppingCart.AddItem(book, 2);
            shoppingCart.RemoveItem(book, 2);

            // Assert
            Assert.AreEqual(0, shoppingCart.LineItems.Count);
        }

        [TestMethod]
        public void LineItem_CalculatePrice_ShouldReturnCorrectPrice()
        {
            // Arrange
            var book = new Book("Test Book", "Description", "Publisher", "ISBN123", 20.0m, 1, 100, "5x8", Book.Languages.English, DateTime.Now);
            var lineItem = new LineItem(book, 3);

            // Act
            var totalPrice = lineItem.Price;

            // Assert
            Assert.AreEqual(60.0m, totalPrice);
        }

    }
}