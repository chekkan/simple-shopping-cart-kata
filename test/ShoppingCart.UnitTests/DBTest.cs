using System;
using Xunit;

namespace ShoppingCart.UnitTests
{
    public class DBTest : IDisposable
    {
        public DBTest()
        {
            Db.Init();
        }

        public void Dispose()
        {
            Db.Close();
        }

        [Fact]
        public void StoreProduct()
        {
            ProductData storedProduct = new ProductData();
            storedProduct.name = "MyProduct";
            storedProduct.price = 1234;
            storedProduct.sku = "999";
            Db.Store(storedProduct);
            ProductData retrievedProduct = Db.GetProductData("999");
            Db.DeleteProductData("999");
            Assert.Equal(storedProduct, retrievedProduct);
        }
    }
}