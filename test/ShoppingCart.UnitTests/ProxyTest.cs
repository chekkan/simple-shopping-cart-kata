using System;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.UnitTests
{
    public class ProxyTest : IDisposable
    {
        public ProxyTest()
        {
            var pd = new ProductData { Sku = "ProxyTest1", Name = "ProxyTestName1", Price = 456 };
            Db.Store(pd);
        }

        [Fact]
        public void ProductProxy()
        {
            Product p = new ProductProxy("ProxyTest1");
            Assert.Equal(456, p.Price);
            Assert.Equal("ProxyTestName1", p.Name);
            Assert.Equal("ProxyTest1", p.Sku);
        }

        public void Dispose()
        {
            Db.DeleteProductData("ProxyTest1");
        }
    }
}