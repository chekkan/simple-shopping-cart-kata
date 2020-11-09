using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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
    }
}