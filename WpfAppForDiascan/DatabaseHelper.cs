using System;
using Npgsql;

namespace WpfAppForDiascan
{
    public class DatabaseHelper : IDisposable
    {
        private readonly NpgsqlConnection _connection;

        public DatabaseHelper(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }

        public void SaveFileHash(string filePath, string hash)
        {
            try
            {
                using (var cmd = new NpgsqlCommand("INSERT INTO file_hashes (file_path, hash) VALUES (@filePath, @hash)", _connection))
                {
                    cmd.Parameters.AddWithValue("filePath", filePath);
                    cmd.Parameters.AddWithValue("hash", hash);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Ошибка сохранения в БД: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
