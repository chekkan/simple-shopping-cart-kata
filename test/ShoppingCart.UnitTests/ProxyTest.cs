using System;
using ShoppingCart.UnitTests;
using Xunit;

public class ProxyTest : IDisposable
{
    public ProxyTest()
    {
        Db.Init();
        ProductData pd = new ProductData();
        pd.Sku = "ProxyTest1";
        pd.Name = "ProxyTestName1";
        pd.Price = 456;
        Db.Store(pd);
    }

    public void Dispose()
    {
        Db.DeleteProductData("ProxyTest1");
        Db.Close();
    }

    [Fact]
    public void TestName()
    {
        Product p = new ProductProxy("ProxyTest1");
        Assert.Equal(456, p.Price);
        Assert.Equal("ProxyTestName1", p.Name);
        Assert.Equal("ProxyTest1", p.Sku);
    }
}