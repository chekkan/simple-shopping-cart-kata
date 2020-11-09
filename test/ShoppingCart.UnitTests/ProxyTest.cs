using System;
using System.Collections.Generic;
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

        [Fact]
        public void OrderProxyTotal()
        {
            Db.Store(new ProductData("Wheaties", 349, "wheaties"));
            Db.Store(new ProductData("Crest", 258, "crest"));
            ProductProxy wheaties = new ProductProxy("wheaties");
            ProductProxy crest = new ProductProxy("crest");
            OrderData od = Db.NewOrder("testOrderProxy");
            OrderProxy order = new OrderProxy(od.OrderId);
            order.AddItem(crest, 1);
            order.AddItem(wheaties, 2);
            Assert.Equal(956, order.Total);
        }

        public void Dispose()
        {
            Db.Clear();
        }
    }

    public class OrderProxy : Order
    {
        private readonly int orderId;

        public OrderProxy(int orderId)
        {
            this.orderId = orderId;
        }

        public void AddItem(ProductProxy productProxy, int qty)
        {
            var id = new ItemData(orderId, qty, productProxy.Sku);
            Db.Store(id);
        }

        public string CustomerId
        {
            get
            {
                OrderData od = Db.GetOrderData(orderId);
                return od.CustomerId;
            }
        }

        public void AddItem(Product p, int quantity)
        {
            ItemData id = new ItemData(orderId, quantity, p.Sku);
            Db.Store(id);
        }

        public int Total
        {
            get
            {
                OrderImp imp = new OrderImp(CustomerId);
                IEnumerable<ItemData> itemDataArray = Db.GetItemsForOrder(orderId);
                foreach (var item in itemDataArray)
                {
                    imp.AddItem(new ProductProxy(item.Sku), item.Qty);
                }

                return imp.Total;
            }
        }

        public int OrderId => orderId;
    }

    public class OrderData
    {
        public string CustomerId { get; }
        public int OrderId { get; }

        public OrderData()
        { }

        public OrderData(int orderId, string customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }

    public class ItemData
    {
        public readonly int OrderId;
        public readonly int Qty;
        public readonly string Sku;

        public ItemData()
        { }

        public ItemData(int orderId, int qty, string sku)
        {
            OrderId = orderId;
            Qty = qty;
            Sku = sku;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ItemData id)
            {
                return OrderId == id.OrderId && Qty == id.Qty && Sku.Equals(id.Sku);
            }

            return false;
        }
    }
}