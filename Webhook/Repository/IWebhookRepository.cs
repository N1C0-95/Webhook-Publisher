
using Webhook.Model.Request;
using Webhook.Model.Response;

namespace Webhook.Repository
{
    public interface IWebhookRepository
    {
        public Task<bool> CreateAsync(WebhookRequest webhook);
        public Task<IEnumerable<WebhookResponse>> GetAllAsync();
        public Task<bool> DeleteAsync(int id);
    }
}
