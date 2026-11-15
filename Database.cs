using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;

namespace morente_sample
{
    internal static class Database
    {
        private const string DatabaseFileName = "morente_Inventory.accdb";

        private static string DatabasePath =>
            Path.Combine(AppContext.BaseDirectory, DatabaseFileName);

        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(
                $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={DatabasePath};Persist Security Info=False;");
        }

        public static void EnsureProductUnitColumn()
        {
            using OleDbConnection connection = GetConnection();
            connection.Open();

            DataTable columns = connection.GetSchema("Columns");
            bool hasProductUnit = columns.Rows.Cast<DataRow>().Any(row =>
                string.Equals(Convert.ToString(row["TABLE_NAME"]), "products", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Convert.ToString(row["COLUMN_NAME"]), "productUnit", StringComparison.OrdinalIgnoreCase));

            if (hasProductUnit)
            {
                return;
            }

            using OleDbCommand command = new(
                "ALTER TABLE products ADD COLUMN productUnit TEXT(255)",
                connection);
            command.ExecuteNonQuery();
        }

        public static void EnsureUsersTable()
        {
            using OleDbConnection connection = GetConnection();
            connection.Open();

            DataTable tables = connection.GetSchema("Tables");
            bool hasUsersTable = tables.Rows.Cast<DataRow>().Any(row =>
                string.Equals(Convert.ToString(row["TABLE_NAME"]), "Users", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Convert.ToString(row["TABLE_TYPE"]), "TABLE", StringComparison.OrdinalIgnoreCase));

            if (hasUsersTable)
            {
                return;
            }

            using OleDbCommand createTable = new(
                "CREATE TABLE Users (UserID AUTOINCREMENT PRIMARY KEY, FullName TEXT(255) NOT NULL, " +
                "Username TEXT(100) NOT NULL, PasswordHash TEXT(64) NOT NULL)",
                connection);
            createTable.ExecuteNonQuery();

            using OleDbCommand createIndex = new(
                "CREATE UNIQUE INDEX UX_Users_Username ON Users (Username)",
                connection);
            createIndex.ExecuteNonQuery();
        }

        public static string HashPassword(string password)
        {
            return Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        }
    }
}
