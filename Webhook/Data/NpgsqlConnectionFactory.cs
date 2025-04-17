using Npgsql;
using System.Data;

namespace Webhook.Data
{
    public class NpgsqlConnectionFactory:IDbConnectionFactory
    {
        private string _connectionString;

        public NpgsqlConnectionFactory(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnetionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
