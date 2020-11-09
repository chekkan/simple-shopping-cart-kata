using System.Collections;
using System.Linq;

namespace ShoppingCart.UnitTests
{
    public interface Order
    {
        string CustomerId { get; }
        void AddItem(Product p, int quantity);
        int Total { get; }
    }

    public class OrderImp : Order
    {
        private readonly ArrayList items = new ArrayList();

        public OrderImp(string cusId)
        {
            CustomerId = cusId;
        }

        public string CustomerId { get; }

        public void AddItem(Product p, int quantity)
        {
            var item = new Item(p, quantity);
            items.Add(item);
        }

        public int Total => (
            from Item item in items
            let p = item.Product
            let qty = item.Quantity
            select p.Price * qty).Sum();
    }

    public class Item
    {
        public readonly Product Product;
        public readonly int Quantity;

        public Item(Product product, int qty)
        {
            Product = product;
            Quantity = qty;
        }
    }

    public interface Product
    {
        int Price { get; }
        string Name { get; }
        string Sku { get; }
    }

    public class ProductImpl : Product
    {
        public int Price { get; }

        public string Name { get; }

        public string Sku { get; }

        public ProductImpl(string sku, string name, int price)
        {
            Sku = sku;
            Name = name;
            Price = price;
        }
    }
}