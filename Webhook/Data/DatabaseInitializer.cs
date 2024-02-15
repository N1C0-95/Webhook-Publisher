using Dapper;

namespace Webhook.Data
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public DatabaseInitializer(IDbConnectionFactory dbConnectionFactory)
        {
            this._connectionFactory = dbConnectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnetionAsync();
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Webhook (
                     Id TEXT PRIMARY KEY,
                     Name TEXT NOT NULL, 
                     TopicName TEXT NOT NULL, 
                     TopicID INTEGER NOT NULL,
                     CallbackUrl TEXT NOT NULL
             )");
        }
    }
}
