using System;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingCart.UnitTests
{
    public class Db
    {
        private static SqlConnection connection;

        public static void Init()
        {
            string connectionString =
                "Server=localhost;" +
                "Database=master;" +
                "user id=sa;password=strong(!)Password;";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public static void Store(ProductData pd)
        {
            SqlCommand command = BuildInsertionCommand(pd);
            command.ExecuteNonQuery();
        }

        private static SqlCommand BuildInsertionCommand(ProductData pd)
        {
            string sql = "INSERT INTO Products VALUES (@sku, @name, @price)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@sku", pd.sku);
            command.Parameters.AddWithValue("@name", pd.name);
            command.Parameters.AddWithValue("@price", pd.price);
            return command;
        }

        public static ProductData GetProductData(string sku)
        {
            SqlCommand command = BuildProductQueryCommand(sku);
            IDataReader reader = ExecuteQueryStatement(command);
            ProductData pd = ExtractProductDataFromReader(reader);
            reader.Close();
            return pd;
        }

        private static SqlCommand BuildProductQueryCommand(string sku)
        {
            string sql = "SELECT * FROM Products WHERE sku = @sku";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@sku", sku);
            return command;
        }

        private static ProductData ExtractProductDataFromReader(IDataReader reader)
        {
            ProductData pd = new ProductData();
            pd.sku = reader["sku"].ToString();
            pd.name = reader["name"].ToString();
            pd.price = Convert.ToInt32(reader["price"]);
            return pd;
        }

        public static void DeleteProductData(string sku)
        {
            BuildProductDeleteStatement(sku).ExecuteNonQuery();
        }

        private static SqlCommand BuildProductDeleteStatement(string sku)
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

        public static void Close()
        {
            connection.Close();
        }
    }
}