using System;
using Xunit;

namespace ShoppingCart.UnitTests
{
    public class DbTest : IDisposable
    {
        [Fact]
        public void StoreProduct()
        {
            ProductData storedProduct = new ProductData { Name = "MyProduct", Price = 1234, Sku = "999" };
            Db.Store(storedProduct);
            ProductData retrievedProduct = Db.GetProductData("999");
            Assert.Equal(storedProduct, retrievedProduct);
        }

        [Fact]
        public void OrderKeyGeneration()
        {
            OrderData o1 = Db.NewOrder("Bob");
            OrderData o2 = Db.NewOrder("Bill");
            int firstOrderId = o1.OrderId;
            int secondOrderId = o2.OrderId;
            Assert.Equal(firstOrderId + 1, secondOrderId);
        }

        [Fact]
        public void StoreItem()
        {
            ItemData storedItem = new ItemData(1, 3, "sku");
            Db.Store(storedItem);
            ItemData[] retrievedItems = Db.GetItemsForOrder(1);
            Assert.Single(retrievedItems);
            Assert.Equal(storedItem, retrievedItems[0]);
        }

        [Fact]
        public void NoItems()
        {
            ItemData[] id = Db.GetItemsForOrder(42);
            Assert.Empty(id);
        }

        public void Dispose()
        {
            Db.Clear();
        }
    }
}