using Microsoft.Data.SqlClient;

namespace FitnessDL
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=GymTest;Integrated Security=True;TrustServerCertificate=True";

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}");
                }
            }
        }
    }
}
