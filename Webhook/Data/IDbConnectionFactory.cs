using System.Data;

namespace Webhook.Data
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnetionAsync();
    }
}
