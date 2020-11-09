using System.Collections;

namespace ShoppingCart.UnitTests
{
    public class Order
    {
        private ArrayList items = new ArrayList();

        public Order(string name)
        { }

        public int Total
        {
            get
            {
                int total = 0;
                foreach (Item item in items)
                {
                    Product p = item.Product;
                    int qty = item.Quantity;
                    total += p.Price * qty;
                }
                return total;
            }
        }

        public void AddItem(Product p, int qty)
        {
            Item item = new Item(p, qty);
            items.Add(item);
        }
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