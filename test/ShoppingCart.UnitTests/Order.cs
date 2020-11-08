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

    public class Product
    {
        public readonly int Price;

        public Product(string name, int price)
        {
            Price = price;
        }
    }

    // public class AddItemTransaction : Transaction
    // {
    //     public void AddItem(int orderId, string sku, int qty)
    //     {
    //         string sql = "insert into items values(" +
    //             orderId + "," + sku + "," + qty + ")";
    //         SqlCommand command = new SqlCommand(sql, connection);
    //         command.ExecuteNonQuery();
    //     }
    // }
}