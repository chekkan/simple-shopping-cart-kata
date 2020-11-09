using Xunit;

namespace ShoppingCart.UnitTests
{
    public class DbTest
    {
        [Fact]
        public void StoreProduct()
        {
            ProductData storedProduct = new ProductData { Name = "MyProduct", Price = 1234, Sku = "999" };
            Db.Store(storedProduct);
            ProductData retrievedProduct = Db.GetProductData("999");
            Db.DeleteProductData("999");
            Assert.Equal(storedProduct, retrievedProduct);
        }
    }
}