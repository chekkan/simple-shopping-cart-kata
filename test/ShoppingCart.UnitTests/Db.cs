using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingCart.UnitTests
{
    public static class Db
    {
        private const string ConnectionString = "Server=localhost,9433;" +
                                                "Database=master;" +
                                                "user id=sa;password=strong(!)Password;";

        public static void Store(ProductData pd)
        {
            using var conn = new SqlConnection(ConnectionString);
            SqlCommand command = BuildInsertionCommand(pd, conn);
            conn.Open();
            command.ExecuteNonQuery();
        }

        private static SqlCommand BuildInsertionCommand(ProductData pd, SqlConnection connection)
        {
            const string sql = "INSERT INTO Products VALUES (@sku, @name, @price)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@sku", pd.Sku);
            command.Parameters.AddWithValue("@name", pd.Name);
            command.Parameters.AddWithValue("@price", pd.Price);
            return command;
        }

        public static void Store(ItemData id)
        {
            using var conn = new SqlConnection(ConnectionString);
            SqlCommand command = BuildItemInsertionStatement(id, conn);
            conn.Open();
            command.ExecuteNonQuery();
        }

        private static SqlCommand BuildItemInsertionStatement(ItemData id, SqlConnection connection)
        {
            const string sql = "INSERT INTO Items(orderId,quantity,sku) VALUES (@orderId, @quantity, @sku)";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@orderId", id.OrderId);
            command.Parameters.AddWithValue("@quantity", id.Qty);
            command.Parameters.AddWithValue("@sku", id.Sku);
            return command;
        }

        public static ProductData GetProductData(string sku)
        {
            using var conn = new SqlConnection(ConnectionString);
            SqlCommand command = BuildProductQueryCommand(sku, conn);
            conn.Open();
            IDataReader reader = ExecuteQueryStatement(command);
            ProductData pd = ExtractProductDataFromReader(reader);
            reader.Close();
            return pd;
        }

        private static SqlCommand BuildProductQueryCommand(string sku, SqlConnection connection)
        {
            const string sql = "SELECT * FROM Products WHERE sku = @sku";
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@sku", sku);
            return command;
        }

        private static ProductData ExtractProductDataFromReader(IDataReader reader)
        {
            ProductData pd = new ProductData
            {
                Sku = reader["sku"].ToString(),
                Name = reader["name"].ToString(),
                Price = Convert.ToInt32(reader["price"])
            };
            return pd;
        }

        public static ItemData[] GetItemsForOrder(int orderId)
        {
            using var conn = new SqlConnection(ConnectionString);
            SqlCommand command = BuildItemsForOrderQueryCommand(orderId, conn);
            conn.Open();
            IDataReader reader = command.ExecuteReader();
            ItemData[] items = ExtractItemDataFromResultSet(reader);
            reader.Close();
            return items;
        }

        private static SqlCommand BuildItemsForOrderQueryCommand(int orderId, SqlConnection conn)
        {
            const string sql = "SELECT * FROM Items WHERE orderId = @orderId";
            var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@orderId", orderId);
            return command;
        }

        private static ItemData[] ExtractItemDataFromResultSet(IDataReader reader)
        {
            var items = new ArrayList();
            while (reader.Read())
            {
                var orderId = Convert.ToInt32(reader["orderId"]);
                var quantity = Convert.ToInt32(reader["quantity"]);
                var sku = reader["sku"].ToString();
                var id = new ItemData(orderId, quantity, sku);
                items.Add(id);
            }

            return (ItemData[]) items.ToArray(typeof (ItemData));
        }

        public static OrderData GetOrderData(int orderId)
        {
            const string sql = "SELECT cusId FROM Orders WHERE orderId = @orderId";
            using var conn = new SqlConnection(ConnectionString);
            var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@orderId", orderId);
            conn.Open();
            IDataReader reader = command.ExecuteReader();
            OrderData od = null;
            if (reader.Read())
                od = new OrderData(orderId, reader["cusId"].ToString());
            reader.Close();
            return od;
        }

        public static void DeleteProductData(string sku)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            BuildProductDeleteStatement(sku, conn).ExecuteNonQuery();
        }

        private static SqlCommand BuildProductDeleteStatement(string sku, SqlConnection connection)
        {
            string sql = "DELETE from Products WHERE sku = @sku";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@sku", sku);
            return command;
        }

        private static IDataReader ExecuteQueryStatement(SqlCommand command)
        {
            IDataReader reader = command.ExecuteReader();
            reader.Read();
            return reader;
        }

        public static OrderData NewOrder(string customerId)
        {
            const string sql = "INSERT INTO Orders(cusId) VALUES(@cusId);" +
                               "SELECT scope_identity()";
            using var conn = new SqlConnection(ConnectionString);
            var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@cusId", customerId);
            conn.Open();
            var newOrderId = Convert.ToInt32(command.ExecuteScalar());
            return new OrderData(newOrderId, customerId);
        }

        public static void Clear()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            ExecuteSql("DELETE FROM Items", conn);
            ExecuteSql("DELETE FROM Orders", conn);
            ExecuteSql("DELETE FROM Products", conn);
        }

        private static void ExecuteSql(string sql, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
        }
    }
}