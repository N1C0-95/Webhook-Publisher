using Webhook.Data;
using Webhook.Model.Request;
using Dapper;
using Webhook.Model.Response;

namespace Webhook.Repository
{
    public class WebhookRepository : IWebhookRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public WebhookRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task<bool> CreateAsync(WebhookRequest webhook)
        {
            using var connection = await _dbConnectionFactory.CreateConnetionAsync();

            var listWebook = await connection.QueryAsync(@"SELECT * FROM Webhook");

            var count = listWebook.Count()+1;

            var result = await connection.ExecuteAsync(@"
            INSERT INTO Webhook(Id, Name, TopicName, TopicId, CallbackUrl) 
            VALUES (@Id,@Name, @TopicName, @TopicID, @CallbackUrl)",
            new { Id = Guid.NewGuid(), Name = webhook.Name, TopicName = webhook.TopicName, TopicID= listWebook.Count() + 1, CallbackUrl = webhook.CallbackUrl });
            return result > 0;
        }     

        public async Task<IEnumerable<WebhookResponse>> GetAllAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnetionAsync();
            var result = await connection.QueryAsync<WebhookResponse>(@"SELECT * FROM Webhook");
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = await _dbConnectionFactory.CreateConnetionAsync();
            var deleted = await connection.ExecuteAsync(@"DELETE FROM Webhook WHERE TopicId = @TopicId ", new { TopicId = id });
            return deleted > 0;
        }

    }
}
