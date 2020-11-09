using Xunit;

namespace ShoppingCart.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestOrderPrice()
        {
            Order o = new Order("Bob");
            Product toothpaste = new ProductImpl("Toothpaste1", "Toothpaste", 129);
            o.AddItem(toothpaste, 1);
            Assert.Equal(129, o.Total);
            Product mouthwash = new ProductImpl("Mouthwash1", "Mouthwash", 342);
            o.AddItem(mouthwash, 2);
            Assert.Equal(813, o.Total);
        }
    }
}
